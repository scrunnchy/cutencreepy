using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingRockBefore : MonoBehaviour {

    //Produces the behaviour that a falling rock will no longer be a threat or visible during reversals
    public bool DisapearsAtReverse;
    //Produces the behaviour that a falling rock will only become visible and a threat during reversals
    public bool AppearsAtReverse;
    //store reference to sprite renderer component
    private SpriteRenderer spriteR;

    
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        spriteR = GetComponent<SpriteRenderer>();
        boxC = GetComponent<BoxCollider>();

        if (enemyPlayerCollision == null)
        {
            enemyPlayerCollision = new UnityEvent();
        }

        //Register for reversal.
        Checkpoint.CheckpointReverse.AddListener(ChangeAppearanceOnReverse);

        //disappear if enemy is to appear during reversals
        if (AppearsAtReverse)
        {
            boxC.enabled = false;
            spriteR.enabled = false;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            rb.isKinematic = false; 
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
