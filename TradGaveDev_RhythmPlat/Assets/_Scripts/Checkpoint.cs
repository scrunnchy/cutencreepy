using System;
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

    //define as rversal point
    public bool reversalPoint;

    //track if checkpoint is reached
    public bool Reached { get; private set; }

    private LevelManager LM;
    private GameObject playerObject;
    private CameraStaller cs;
    private bool reversed;

    void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        Reached = false;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        cs = GetComponentInChildren<CameraStaller>();
        reversed = false;
        LevelManager.CheckpointReverse.AddListener(TurnAroundAndReset);
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = playerObject.transform.position;
        //if player is at checkpoint
        if (!Reached 
            && (playerPos.x > (transform.position.x - .1)) 
            && (playerPos.x < (transform.position.x + .1))
            && (playerPos.y > (transform.position.y - 2))
            && (playerPos.y < (transform.position.y + 2)))
        {
            Debug.Log("checkpoint reached");
            Reached = true;
            //consume candy
            GetComponent<SpriteRenderer>().enabled = false;
            //trigger "CheckpointCollision" event
            LevelManager.CheckpointCollision.Invoke();
            //trigger reversal conditionally
            if (reversalPoint)
                LevelManager.CheckpointReverse.Invoke();
        }
    }

    private bool isClose(float y1, float y2)
    {
        if (Math.Abs(y1 - y2) <= 3)
            return true;
        return false;
    }

    
    private void TurnAroundAndReset() //trigger this method when the last checkpoint is reached
    {
        reversed = !reversed;
        //will behave direcitonally if not a reversal point.
        if (!reversalPoint)
        {
            // always flip to the direction not currently facing
            if (!GetComponent<SpriteRenderer>().flipX)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        //if this is not the checkpoint that was reached, then orient the collider correctly.
        if (!Reached)
        {
            Debug.Log("reorienting collider");
            if (reversed)
            {
                cs.boxC.transform.Translate((cs.cameraStallDistance*2), 0f, 0f); 
            }
            else
            {
                cs.boxC.transform.Translate(+(cs.cameraStallDistance * 2), 0f, 0f);
            }
        }
    }
}
