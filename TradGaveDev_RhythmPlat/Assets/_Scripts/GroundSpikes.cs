using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundSpikes : MonoBehaviour
{

    // track if obstacle is expended from hitting player
    private bool isExpended;
    private bool isReversed;

    //store reference to Level Manager
    public LevelManager LM;

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

    //store reference to sprite renderer component
    private SpriteRenderer spriteR;
    private Transform particlesT;

    //private bool isMoving = false;

    // Use this for initialization
    void Start()
    {
        particlesT = transform.parent.GetChild(0);
        if (enemyPlayerCollision == null)
        {
            enemyPlayerCollision = new UnityEvent();
        }
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        spriteR = gameObject.transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>();
        isReversed = false;
        //Register for reversal.
        LevelManager.CheckpointReverse.AddListener(RespondToReverse);
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        bool playerOnRight = (playerPos.x - this.transform.position.x) < spikeTriggerDistance;
        bool playerOnLeft = (this.transform.position.x - playerPos.x) < spikeTriggerDistance;
        bool playerWithinRangeY = playerPos.y <= transform.position.y + 3 && playerPos.y >= transform.position.y - 3;
        bool playerWithinRangeX = playerPos.x <= transform.position.x + .1 && playerPos.x >= transform.position.x - 3;
        if (!LM.isReversed)
        {
            if (!isExpended && playerOnLeft && playerWithinRangeY && playerWithinRangeX)
            {
                isExpended = true;
                //move the spike upwards
                //isMoving = true;
                moveSprite();
            }
        }
        else
        {
            if (!isExpended && playerOnRight && playerWithinRangeY && playerWithinRangeX)
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
    /// </summary>
    private void RespondToReverse()
    {
        isExpended = false;
        isReversed = !isReversed;
        //rotate the particle effect into position for reversal
        
        particlesT.Translate(3f, 0f, 0f);
        particlesT.Rotate(180f, 180f, 0f);
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
                //trigger Damage event
                enemyPlayerCollision.Invoke();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        moveSpriteDown();
    }

    private void moveSprite()
    {
        float step = speed * Time.deltaTime;
        Vector3 up = new Vector3(spriteR.transform.position.x, spriteR.transform.position.y + amountUp, spriteR.transform.position.z);
        StartCoroutine(MoveObject(transform.position, up, step));
    }

    private void moveSpriteDown()
    {
        float step = speed * Time.deltaTime;
        Vector3 down = new Vector3(spriteR.transform.position.x, spriteR.transform.position.y - amountUp, spriteR.transform.position.z);
        StartCoroutine(MoveObject(transform.position, down, step));
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
