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
    Camera mainCam;
    PlatformerCameraFollow follower;
    public float cameraStallDistance = 10;

    //define as rversal point
    public bool reversalPoint;

    //track if checkpoint is reached
    public bool Reached { get; private set; }

    private LevelManager LM;
    private GameObject playerObject;
    private bool cameraIsStalled;

    void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        Reached = false;
        mainCam = Camera.main;
        follower = mainCam.GetComponent<PlatformerCameraFollow>();
        playerObject = GameObject.FindGameObjectWithTag("Player");

        //register for reversal event if not reversal point.
        if (!reversalPoint)
        {
            LevelManager.CheckpointReverse.AddListener(TurnAroundAndReset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = playerObject.transform.position;

        if (!LM.isReversed)
        {
            // if the player is to the left of the checkpoint and the level is not reversed 
            if (playerPos.x >= transform.position.x - cameraStallDistance && playerPos.x < transform.position.x && !Reached && isClose(playerPos.y, transform.position.y))
            {
                stallCam();
            }
            else
                resumeCam();
        }
        else
        {
            // if the player is to the right of the checkpoint and the level is reversed
            if (playerPos.x <= transform.position.x + cameraStallDistance && playerPos.x > transform.position.x && !Reached && isClose(playerPos.y, transform.position.y))
            {
                stallCam();
            }
            else
                resumeCam();
        }
    }

    private bool isClose(float y1, float y2)
    {
        if (Math.Abs(y1 - y2) <= 3)
            return true;
        return false;
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
            LevelManager.CheckpointCollision.Invoke();
            //trigger reversal conditionally
            if (reversalPoint)
                LevelManager.CheckpointReverse.Invoke();
        }
    }

    private void stallCam()
    {
        Debug.Log("Stalled");
        follower.isFollowing = false;
        cameraIsStalled = true;
    }

    private void resumeCam()
    {
        Debug.Log("Resumed");
        follower.isFollowing = true;
        cameraIsStalled = false;
    }

    private void TurnAroundAndReset() //trigger this method when the last checkpoint is reached
    {
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
            //reset
            Reached = false;
        }
    }
}
