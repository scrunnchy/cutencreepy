using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockVisual : MonoBehaviour {

    Vector3 playerPos;
    // Track if in reversal state
    private bool isReversed;
    //rock height at reset
    int initialRockHeight = 6;
    //x distance before activation
    float rockTriggerDistance = 4.4608f - .9f;
    // rigidbody of rock
    Rigidbody rb;

    // Use this for initialization
    void Start ()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isReversed) {
            if ((this.transform.position.x - playerPos.x) < rockTriggerDistance)
            {
                
            }
        }
        else {
            if ((playerPos.x - this.transform.position.x) < rockTriggerDistance)
            {

            }
        }
    }
}
