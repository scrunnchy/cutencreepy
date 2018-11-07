using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class PlatformVerticalMovement : MonoBehaviour {

    // Transforms to act as start and end markers for the journey.
    public Vector3 startMarker;
    public Vector3 endMarker;

    public bool moveUpwards;
    public bool moveDownwards;

    private bool isOriginalHeight;

    private Vector3 platformOriginalHeight;
    public float lerpTime;

    // Movement speed in units/sec.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public GameObject[] waypoints;

	// Use this for initialization
	void Start ()
    {
        platformOriginalHeight = this.transform.position;
        Koreographer.Instance.RegisterForEvents("SingleBeatTrack", movePlatform);
        // Keep a note of the time the movement started.
        startTime = Time.time;

        //lerpTime = 60 / (float)Koreographer.Instance.GetMusicBPM();
        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker, endMarker);
    }
	
	// Update is called once per frame
	void Update() {

        journeyLength = Vector3.Distance(startMarker, endMarker);
        //Debug.Log(journeyLength);
        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;
        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / journeyLength;
        //Debug.Log(fracJourney);
        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
    }

    void movePlatform(KoreographyEvent evt)
    {
        //Debug.Log("move platform entered");
         if (moveUpwards)
         {
             if ((platformOriginalHeight.y - transform.position.y) < .5)
             {
                 Debug.Log("move platform part 1");
                 // move from platform original position to top position
                 //transform.position = Vector3.Lerp(platformOriginalHeight, waypoints[1].transform.position, (lerpTime));
                 startMarker = platformOriginalHeight;
                 endMarker = waypoints[1].transform.position;
                 startTime = Time.time;
                 moveUpwards = false;
                 moveDownwards = true;
             }
             else
             {
                 Debug.Log("move platform part 2");
                 endMarker = platformOriginalHeight;
                 startMarker = waypoints[0].transform.position;
                 startTime = Time.time;
                 // move from bottom position to platform original position
                 //transform.position = Vector3.Lerp(waypoints[1].transform.position, platformOriginalHeight, (lerpTime));
             }
         }
         else if (moveDownwards)
         {
             if ((transform.position.y - platformOriginalHeight.y) < .5)
             {
                 Debug.Log("move platform part 3");
                 //transform.position = Vector3.Lerp(platformOriginalHeight, waypoints[0].transform.position, (lerpTime));
                 // move from platform position to bottom position
                 startMarker = platformOriginalHeight;
                 endMarker = waypoints[0].transform.position;
                 startTime = Time.time;
                 moveUpwards = true;
                 moveDownwards = false;
             }
             else
             {
                // move from bottom position to platform position
                 endMarker = platformOriginalHeight;
                 startMarker = waypoints[1].transform.position;
                 startTime = Time.time;
                 Debug.Log("move platform part 4");
                 //transform.position = Vector3.Lerp(waypoints[0].transform.position, platformOriginalHeight, (lerpTime));
             }
         }
    }
}





/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class PlatformVerticalMovement : MonoBehaviour
{

    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;

    public bool moveUpwards;
    public bool moveDownwards;

    private bool isOriginalHeight;

    private Transform platformOriginalHeight;
    public float lerpTime;

    // Movement speed in units/sec.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public GameObject[] waypoints;

    // Use this for initialization
    void Start()
    {
        platformOriginalHeight = this.transform;
        Koreographer.Instance.RegisterForEvents("EveryOtherBeatTrack", movePlatform);
        // Keep a note of the time the movement started.
        startTime = Time.time;

        //lerpTime = 60 / (float)Koreographer.Instance.GetMusicBPM();
        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;
        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / journeyLength;
        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
    }

    void movePlatform(KoreographyEvent evt)
    {
        //Debug.Log("move platform entered");
        if (moveUpwards)
        {
            if ((platformOriginalHeight.position.y - transform.position.y) < .5)
            {
                Debug.Log("move platform part 1");
                // move from platform original position to top position
                //transform.position = Vector3.Lerp(platformOriginalHeight, waypoints[1].transform.position, (lerpTime));
                startMarker = platformOriginalHeight;
                endMarker = waypoints[1].transform;
                startTime = Time.time;
                moveUpwards = false;
                moveDownwards = true;
            }
            else
            {
                Debug.Log("move platform part 2");
                endMarker = platformOriginalHeight;
                startMarker = waypoints[1].transform;
                startTime = Time.time;
                // move from bottom position to platform original position
                //transform.position = Vector3.Lerp(waypoints[1].transform.position, platformOriginalHeight, (lerpTime));
            }
        }
        else if (moveDownwards)
        {
            if ((transform.position.y - platformOriginalHeight.position.y) < .5)
            {
                Debug.Log("move platform part 3");
                //transform.position = Vector3.Lerp(platformOriginalHeight, waypoints[0].transform.position, (lerpTime));
                startMarker = platformOriginalHeight;
                endMarker = waypoints[0].transform;
                startTime = Time.time;
                moveUpwards = true;
                moveDownwards = false;
            }
            else
            {
                endMarker = platformOriginalHeight;
                startMarker = waypoints[0].transform;
                startTime = Time.time;
                Debug.Log("move platform part 4");
                //transform.position = Vector3.Lerp(waypoints[0].transform.position, platformOriginalHeight, (lerpTime));
            }
        }
    }
}
*/
