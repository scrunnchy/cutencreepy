using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundSpikes : MonoBehaviour {

    // track if obstacle is expended from hitting player
    private bool isExpended;
    //store reference to Level Manager
    public LevelManager LM;
    //Produces the behaviour that a falling rock will no longer be a threat or visible during reversals
    public bool DisapearsAtReverse;
    //Produces the behaviour that a falling rock will only become visible and a threat during reversals
    public bool AppearsAtReverse;
    //The speed at which the spikes will move up.
    public float speed = 1f;
    //The amount of units the spikes will move up
    public float amountUp = 7f;
    //set the distanc at which the player should be as the spike jumps up
    public float spikeTriggerDistance = 2f;
    //use this value for the player's current position
    private Vector3 playerPos;
    //create event for damage "enemyPlayerCollision"
    public static UnityEvent enemyPlayerCollision;

    //store the reversal state shown in the level manager
    private bool isReversed;
    //store reference to sprite renderer component
    private SpriteRenderer spriteR;
    //store reference to box collider component
    private BoxCollider boxC;

    //private bool isMoving = false;

    // Use this for initialization
    void Start () {

        if (enemyPlayerCollision == null)
        {
            enemyPlayerCollision = new UnityEvent();
        }
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        isReversed = LM.isReversed;
        spriteR = gameObject.transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>();
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
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (!isReversed)
        {
            if (!isExpended && ((this.transform.position.x - playerPos.x) < spikeTriggerDistance))
            {
                isExpended = true;
                //move the spike upwards
                //isMoving = true;
                moveSprite();
            }
        }
        else
        {
            if (!isExpended && ((playerPos.x - this.transform.position.x) < spikeTriggerDistance))
            {
                isExpended = true;
                //move the spike upwards
                //isMoving = true;
                moveSprite();
            }
        }
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
        float step = speed * Time.deltaTime;
        Vector3 up = new Vector3(spriteR.transform.position.x, spriteR.transform.position.y + amountUp, spriteR.transform.position.z);
        //spriteR.transform.position = Vector3.Lerp(transform.position, up, step);
        //if (transform.position.y >= up.y)
        //    isMoving = false;

        StartCoroutine(MoveObject(transform.position, up, step));
    }

    IEnumerator MoveObject(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            spriteR.transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        spriteR.transform.position = target;
    }
}
