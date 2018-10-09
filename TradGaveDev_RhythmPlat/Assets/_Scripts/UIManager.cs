using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public LevelManager LM;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        checkForKeyStroke();
    }

    private void checkForKeyStroke()
    {
        if (Input.GetButtonDown("pauseButton"))
            LM.togglePause();
    }
}
