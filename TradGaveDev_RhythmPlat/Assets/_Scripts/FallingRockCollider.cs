using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingRockCollider : MonoBehaviour {
    //store reference to box collider component
    private BoxCollider boxC;
    
    // Use this for initialization
    void Start () {

        boxC = GetComponent<BoxCollider>();

    }

    /// <summary>
    /// Check for player within danger zone box collider
    /// If player detected AND they are not sliding, trigger damage event.
    /// an obstacle will only be able to deal damage once, and then it is expended until reversal.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            PlayerControl playerInfo = collider.gameObject.GetComponent<PlayerControl>();
            // check if the layer is in any valid avoid state.
            if (!playerInfo._inSlide)
            {
                //Debug.Log("damage dealt");
                //trigger Damage event
                LevelManager.enemyPlayerCollision.Invoke();
            }
        }

    }

}
