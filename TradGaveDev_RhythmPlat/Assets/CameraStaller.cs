using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStaller : MonoBehaviour
{

    Camera mainCam;
    PlatformerCameraFollow follower;
    public float cameraStallDistance = 5;
    public float cameraResumeDistance = 5;

    private LevelManager LM;
    private GameObject playerObject;
    private bool cameraIsStalled;
    private bool colliderOnLeft;
    private BoxCollider boxC;
    // Use this for initialization
    void Start()
    {
        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        mainCam = Camera.main;
        follower = mainCam.GetComponent<PlatformerCameraFollow>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        boxC = gameObject.GetComponent<BoxCollider>();
        cameraIsStalled = false;
        colliderOnLeft = true;

        //set collider to the x position defined by the stall distance.
        boxC.transform.Translate(-cameraStallDistance, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            //stallCam in the two cases that fit:
            //camera not stalled, collider on left, not reversed OR camera not stalled, collider on right, reversed
            if ((!cameraIsStalled && colliderOnLeft && !LM.isReversed) || (!cameraIsStalled && !colliderOnLeft && LM.isReversed))
            {
                //player has entered checkpoint's range for the first time
                stallCam();
                //sinch collider to cameraResumeDistance
                SinchToResumeDistance();
            }
            else if ((cameraIsStalled && colliderOnLeft && LM.isReversed) || (cameraIsStalled && !colliderOnLeft && !LM.isReversed))
            {
                resumeCam();
                //change collider to other side at collider distance now that things are reversed. 
                FlipCollider();
            }
        }
    }

    private void FlipCollider()
    {
        Debug.Log("fliping collider");
        if (colliderOnLeft)
        {
            //move collider to the right side
            boxC.transform.Translate(cameraStallDistance + cameraResumeDistance, 0f, 0f);
            colliderOnLeft = false;
        } 
        else
        {
            //move collider to the left side
            boxC.transform.Translate(-(cameraStallDistance + cameraResumeDistance), 0f, 0f);
            colliderOnLeft = true;
        }
    }

    private void SinchToResumeDistance()
    {
        float distanceToMove;
        //camera has just stalled, need to preserve the side.
        //calculate distance to move
        distanceToMove = cameraStallDistance - cameraResumeDistance;
        //determine direction to move based on side
        if (colliderOnLeft)
        {
            boxC.transform.Translate(distanceToMove, 0f, 0f);
        }
        else
        {
            boxC.transform.Translate(-distanceToMove, 0f, 0f);
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
}
