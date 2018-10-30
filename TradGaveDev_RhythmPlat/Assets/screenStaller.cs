using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenStaller : MonoBehaviour {

    Camera mainCam;

    // Use this for initialization
    void Start () {
        GameObject mainObject = GameObject.Find("Main Camera");
        mainCam = mainObject.GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter()
    {
        stallCam();
    }
    
    void OnTriggerStay()
    {

    }

    void OnTriggerExit()
    {
        resumeCam();
    }

    private void stallCam()
    {

    }

    private void resumeCam()
    {

    }
}
