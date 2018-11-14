using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockVisual : MonoBehaviour {

    Vector3 originalRockPos;

    Vector3 playerPos;
    // Track if in reversal state
    public bool isReversed;
    // Track if rock is currently falling
    private bool isFalling;
    // If rock is expended, it will not trigger again until reversal is done
    private bool isExpended;
    // Distance at which rock will be triggered to fall
    public float rockTriggerDistance = 6f;
    // rigidbody of rock
    private Rigidbody rb;
    // delay in which the rock's position gets reset to its original position
    float rockResetDelay = 2f;
    private GameObject playerObject;

    private SpriteRenderer sprite;
    private Transform particlesT;

    // Use this for initialization
    void Start ()
    {
        sprite = GetComponent<SpriteRenderer>();
        particlesT = transform.parent.GetChild(2);
        if (isReversed)
        {
            sprite.flipX = true;
        }
        // Get the position we will reset the rock to later
        originalRockPos = gameObject.transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        // If reversal is active we want to flip the rock's trigger point
        LevelManager.CheckpointReverse.AddListener(flipRockOrientation);
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Constantly update playe rposition so rock can accurately tell if it should fall
        playerPos = playerObject.transform.position;
        //bool playerWithinRangeY = playerPos.y <= transform.position.y + 0 && playerPos.y >= transform.position.y - 5;
        // We go into the if statement if we are not in reversal and we can still make the rock fall
        if (!isReversed && !isExpended) {
            if ((this.transform.position.x - playerPos.x) < rockTriggerDistance )
            {
                isExpended = true;
                isFalling = true;
                rb.useGravity = true;
            }
        }
        // else we are either in reversal or rock is expended. 
        else {
            if ((playerPos.x - this.transform.position.x) < rockTriggerDistance +.1 && !isExpended)
            {
                isExpended = true;
                isFalling = true;
                rb.useGravity = true;
            }
        }
        // if statement executed when the rock is falling. We have the second part of the statement so 
        // that we don't "reset rock" when the rock hasn't begun to fall yet.
        if (isFalling && this.transform.position.y < playerPos.y)
        {
            if (rb.velocity.y == 0)
            {
                isFalling = false;
                StartCoroutine("ResetRock");
            }
        }
    }
    private void flipRockOrientation()
    {
        isExpended = false;
        isReversed = true;
        //rotate the particle effect into position for reversal
        if (isReversed)
        {
            particlesT.Translate(3f, 0f, 0f);
            particlesT.Rotate(180f, 180f, 0f);
        }
        else
        {
            particlesT.Translate(-3f, 0f, 0f);
            particlesT.Rotate(180f, 180f, 0f);
        }
    }
    IEnumerator ResetRock()
    {
        yield return new WaitForSeconds(rockResetDelay);
        this.transform.position = originalRockPos;
        rb.useGravity = false;
    }
}
