using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockVisual : MonoBehaviour {

    Vector3 originalRockPos;

    Vector3 playerPos;
    // Track if in reversal state
    private bool isReversed;

    private bool isExpended;
    //rock height at reset
    int initialRockHeight = 6;
    //x distance before activation
    float rockTriggerDistance = 4.4608f - .9f;
    // rigidbody of rock
    Rigidbody rb;

    float rockResetDelay = 6f;

    // Use this for initialization
    void Start ()
    {
        originalRockPos = gameObject.transform.position;
        Rigidbody rb = this.GetComponent<Rigidbody>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Debug.Log(this.transform.position.x - playerPos.x);
        if (!isReversed) {
            if ((this.transform.position.x - playerPos.x) < rockTriggerDistance)
            {
                rb.useGravity = true;
            }
        }
        else {
            if ((playerPos.x - this.transform.position.x) < rockTriggerDistance)
            {
                rb.useGravity = true;
            }
        }
       
    }
    private bool isFalling()
    {
        return false;
    }
    private bool Grounded()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out raycastHit, 1.2f))
        {
            if (raycastHit.collider.gameObject != this.gameObject)
            { 
                return true;
            }
        }
        return false;
    }
    IEnumerator resetRock()
    {
        yield return new WaitForSeconds(rockResetDelay);
        this.transform.position = originalRockPos;
        rb.useGravity = false;
        isExpended = false;
    }
}
