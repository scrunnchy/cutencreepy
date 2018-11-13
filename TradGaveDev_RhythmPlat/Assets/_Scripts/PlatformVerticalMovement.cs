using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class PlatformVerticalMovement : MonoBehaviour {

    // Vector3's to act as start and end markers for the journey.
    private Vector3 startMarker;
    private Vector3 endMarker;

    public bool moveUpwards;
    public bool moveDownwards;

    private Vector3 platformOriginalHeight;

    // Movement speed in units/sec.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;
    
    // array that will hold the different positions the platform will go to
    private Vector3[] points;

    // amount of meters we want to offset the positions
    public float yOffset = 0;

    private Vector3 playerPos;

    public bool showVelocity;

	// Use this for initialization
	void Start ()
    {
        // initialize point array we will use to contain positions 
        points = new Vector3[10];
        // top platform position
        points[1] = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        // bottom platform position
        points[0] = new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z);
        // get the platform's original height/position
        platformOriginalHeight = this.transform.position;
        // register for koreography
        Koreographer.Instance.RegisterForEvents("SingleBeatTrack", movePlatform);

        // Keep a note of the time the movement started.
        startTime = Time.time;
        
        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker, endMarker);

        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (moveDownwards)
        {
            startMarker = platformOriginalHeight;
            endMarker = points[0];
            // Now that we are at the bottom, set values so we can go back up
            moveUpwards = true;
            moveDownwards = false;
        }
        else
        {
            // start at original platform position
            startMarker = platformOriginalHeight;
            // end at top position
            endMarker = points[1];
            // Now that we are at the top, set values so we can go back down
            moveUpwards = false;
            moveDownwards = true;
        }
    }
	
	// Update is called once per frame
	void Update()
    {
        journeyLength = Vector3.Distance(startMarker, endMarker);
        //Debug.Log(journeyLength);
        // Distance moved = time * speed.
        float distCovered = ((Time.time - startTime) * speed/8);
        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / journeyLength;
        //if (showVelocity)
            //Debug.Log(fracJourney);
        //Debug.Log(fracJourney);
        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
    }

    void movePlatform(KoreographyEvent evt)
    {
        //Debug.Log("move platform entered");

        // If we go into this code, we are either currently at bottom or regular platform positions
        if (moveUpwards)
         {
             if ((platformOriginalHeight.y - transform.position.y) < .5)
             {
                 // move from platform original position to top position

                 // start at original platform position
                 startMarker = platformOriginalHeight;
                // end at top position
                 endMarker = points[1];
                 startTime = Time.time;
                // Now that we are at the top, set values so we can go back down
                 moveUpwards = false;
                 moveDownwards = true;
             }
             else
             {
                // move from bottom position to platform original position

                // end at regular platform height
                 endMarker = platformOriginalHeight;
                // start at bottom position
                 startMarker = points[0];
                 startTime = Time.time;
             }
         }
         // If we go into this code, we are either currently at top or regular platform positions
         else if (moveDownwards)
         {
             if ((transform.position.y - platformOriginalHeight.y) < .5)
             {
                // move from original platform position to bottom position

                // start at original platform position
                 startMarker = platformOriginalHeight;
                // end at bottom platform position
                endMarker = points[0];
                 startTime = Time.time;

                // Now that we are at the bottom, set values so we can go back up
                moveUpwards = true;
                 moveDownwards = false;
             }
             else
             {
                // move from top position to original platform position

                // end at original platform position
                endMarker = platformOriginalHeight;
                // start at top platform position
                startMarker = points[1];
                 startTime = Time.time;
             }
         }

    }
}
