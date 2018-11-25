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
            && (playerPos.y > (transform.position.y - 3))
            && (playerPos.y < (transform.position.y + 3)))
        {
            Debug.Log("checkpoint reached");
            Reached = true;
            //trigger "CheckpointCollision" event
            LevelManager.CheckpointCollision.Invoke();
            //trigger reversal conditionally
            if (reversalPoint)
                LevelManager.CheckpointReverse.Invoke();
            //consume candy
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void TurnAroundAndReset() 
    {
        reversed = !reversed;
        
    }
}
