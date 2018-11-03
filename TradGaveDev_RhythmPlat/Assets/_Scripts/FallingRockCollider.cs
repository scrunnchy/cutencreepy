using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingRockCollider : MonoBehaviour {
    // track if the rock is expended from hitting player
    private bool isExpended;
    //store reference to box collider component
    private BoxCollider boxC;
    //create event for damage "enemyPlayerCollision"
    public static UnityEvent enemyPlayerCollision;

    // Use this for initialization
    void Start () {

        boxC = GetComponent<BoxCollider>();
        

        if (enemyPlayerCollision == null)
        {
            enemyPlayerCollision = new UnityEvent();
        }

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
            if (!playerInfo._inSlide)
            {
                Debug.Log("damage dealt");
                //trigger Damage event
                enemyPlayerCollision.Invoke();
                isExpended = true;
            }
        }

    }
    /*
     * CODE FROM FALLING ROCK BEFORE 
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
 */
}
