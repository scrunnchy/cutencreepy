using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenStaller : MonoBehaviour
{

    Camera mainCam;
    PlatformerCameraFollow follower;

    // Use this for initialization
    void Start()
    {
        mainCam = Camera.main;
        follower = mainCam.GetComponent<PlatformerCameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Player"/* && !Expended*/)
            stallCam();
    }

    void OnTriggerStay()
    {

    }

    void OnTriggerExit(Collider player)
    {
        if(player.tag == "Player"/* && !Expended*/)
            resumeCam();
    }

    private void stallCam()
    {
        follower.isFollowing = false;
    }

    private void resumeCam()
    {
        follower.isFollowing = true;
    }
}
