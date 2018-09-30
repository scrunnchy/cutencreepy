using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour {

    UnityEvent conductor = new UnityEvent();

	// Use this for initialization
	void Start () {
        conductor.AddListener(enemyPlayerCollision);
        conductor.AddListener(checkpointCollision);
        conductor.AddListener(composerEvent);
	}

    // Update is called once per frame
    void Update () {
		
	}

    private void enemyPlayerCollision()
    {
        throw new NotImplementedException();
    }

    private void composerEvent()
    {
        throw new NotImplementedException();
    }

    private void checkpointCollision()
    {
        throw new NotImplementedException();
    }
}
