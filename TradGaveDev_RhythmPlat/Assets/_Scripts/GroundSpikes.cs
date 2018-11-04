using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundSpikes : MonoBehaviour {

    // track if obstacle is expended from hitting player
    private bool isExpended;
    //Produces the behaviour that a falling rock will no longer be a threat or visible during reversals
    public bool DisapearsAtReverse;
    //Produces the behaviour that a falling rock will only become visible and a threat during reversals
    public bool AppearsAtReverse;
    //The speed at which the spikes will move up.
    public float speed = 1f;
    //The amount of units the spikes will move up
    public float amountUp = 7f;

    //create event for damage "enemyPlayerCollision"
    public static UnityEvent enemyPlayerCollision;

    //store reference to sprite renderer component
    private SpriteRenderer spriteR;
    //store reference to box collider component
    private BoxCollider boxC;

    // Use this for initialization
    void Start () {

        if (enemyPlayerCollision == null)
        {
            enemyPlayerCollision = new UnityEvent();
        }

        spriteR = GetComponent<SpriteRenderer>();
        boxC = GetComponent<BoxCollider>();

        //Register for reversal.
        Checkpoint.CheckpointReverse.AddListener(ChangeAppearanceOnReverse);

        //disappear if enemy is to appear during reversals
        if (AppearsAtReverse)
        {
            boxC.enabled = false;
            spriteR.enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// triggers on reversal events
    /// enables/disables this obstacle's box collider and sprite renderer based on settings.
    /// </summary>
    private void ChangeAppearanceOnReverse()
    {
        //appear if invisable and dissapear if set to toggle upon reversal.
        if (AppearsAtReverse || DisapearsAtReverse)
        {
            if (spriteR.enabled)
            {
                boxC.enabled = false;
                spriteR.enabled = false;
            }
            else
            {
                boxC.enabled = true;
                spriteR.enabled = true;
            }
        }
        isExpended = false;
    }

    /// <summary>
    /// Check for player within danger zone box collider
    /// If player detected AND they are not sliding, trigger damage event.
    /// an obstacle will only be able to deal damage once, and then it is expended until reversal.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerStay(Collider collider)
    {
        if (!isExpended && collider.gameObject.tag == "Player")
        {
            moveSprite();
            PlayerControl playerInfo = collider.gameObject.GetComponent<PlayerControl>();
            // check if the layer is in any valid avoid state.
            if (!playerInfo._inJump)
            {
                Debug.Log("damage dealt");
                //trigger Damage event
                enemyPlayerCollision.Invoke();
                isExpended = true;
            }
        }
        
    }

    private void moveSprite()
    {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        float step = speed * Time.deltaTime;
        Vector3 up = new Vector3(sprite.transform.position.x, sprite.transform.position.y + amountUp, sprite.transform.position.z);
        sprite.transform.position = Vector3.Lerp(transform.position, up, step); 
    }
}
