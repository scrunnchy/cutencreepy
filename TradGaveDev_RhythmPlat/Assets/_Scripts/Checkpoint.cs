using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is a component for all checkpoints in the game.
/// The class will respond to player collision by changing its "Reached" field and sending a collision event
/// Additionally, it reverses its sprite and resets its "Reached" field
/// </summary>
public class Checkpoint : MonoBehaviour
{
    //create collision event for reaching the checkpoint and reversal event.
    public static UnityEvent CheckpointCollision = new UnityEvent();
    public static UnityEvent CheckpointReverse = new UnityEvent();
    //define as rversal point
    public bool reversalPoint;

    //track if checkpoint is reached
    public bool Reached { get; private set; }

    
    void Start()
    {

        Reached = false;

        //register for reversal event if not reversal point.
        if (!reversalPoint)
        {
            CheckpointReverse.AddListener(TurnAroundAndReset);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// triggers the "CheckpointCollision" event once and adjusts public field
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if (!Reached && collider.gameObject.tag == "Player")
        {
            Debug.Log("checkpoint reached");
            Reached = true;
            //trigger "CheckpointCollision" event
            CheckpointCollision.Invoke();
            //trigger reversal conditionally
            if (reversalPoint)
            {
                CheckpointReverse.Invoke();
            }
        }
    }

    private void TurnAroundAndReset() //trigger this method when the last checkpoint is reached
    {
        if (!GetComponent<SpriteRenderer>().flipX) // if it has not already been flipped
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        //reset
        Reached = false;
    }
}
