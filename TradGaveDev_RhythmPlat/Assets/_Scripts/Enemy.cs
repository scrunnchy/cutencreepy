using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {
    // store enemy collider and isExpended
    private bool isExpended;

    // create event for damage "enemyPlayerCollision"
    public static UnityEvent enemyPlayerCollision = new UnityEvent();

	// On start, an enemy will:
    // retrieve collider component
    // register as an event listener for beats and level reversal
	void Start () {

        //Register for koreography beats.
        Koreographer.Instance.RegisterForEvents("SingleBeatTrack", IdleAnimation);
    }
	
	// Once per frame, 
	void Update () {

	}

    /// <summary>
    /// Check for player within danger zone box collider
    /// If player detected AND they are not dodging, trigger damage event.
    /// an enemy will only bbe able to deal damage once, and then it is expended until reversal.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerStay(Collider collider)
    {
        if (!isExpended && collider.gameObject.tag == "Player")
        {
            //TODO: change player script and bool to real type/name
            TestPlayerDummy playerInfo = collider.gameObject.GetComponent<TestPlayerDummy>();
            // check the boolean isDodging.
            if (!playerInfo.isDodging)
            {
                Debug.Log("damage dealt");
                //trigger Damage event
                enemyPlayerCollision.Invoke();
                isExpended = true;
            }
        }
    }

    /// <summary>
    /// On the relevant beat, an enemy will initiate its dance animation.
    /// </summary>
    /// <param name="beat"></param>
    private void IdleAnimation(KoreographyEvent beat)
    {
        //Dance (blink on and off)
        SpriteRenderer enemySpriteRen = (SpriteRenderer)GetComponent("SpriteRenderer");
        enemySpriteRen.enabled = enemySpriteRen.enabled ? false : true;
    }

    /// <summary>
    /// Triggers as soon as the reversal point is reached.
    /// Flips the enemy sprite such that it is facing the player.
    /// Once flipped, the sprite will not re-flip. 
    /// resets the enemy to be no longer expended. 
    /// </summary>
    private void TurnAround() 
    {
        if (!GetComponent<SpriteRenderer>().flipX) // if it has not already been flipped
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        isExpended = false;
    }
}
