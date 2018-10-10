using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    UnityEvent conductor = new UnityEvent();
    public bool paused { get; private set; }

    Camera pauseCamera;

    private void Awake()
    {
        paused = false;
    }

    // Use this for initialization
    void Start()
    {
        //Get camera and disable 
        GameObject pauseObject = GameObject.Find("Pause Camera");
        if (pauseObject != null)
            pauseCamera = pauseObject.GetComponent<Camera>();
        pauseCamera.enabled = false;

        //Add listeners for collision events
        conductor.AddListener(enemyPlayerCollision);
        conductor.AddListener(checkpointCollision);
        conductor.AddListener(composerEvent);

    }

    // Update is called once per frame
    void Update()
    {

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

/// <summary>
/// Toggles the pause state
/// </summary>
/// <returns></returns>
    public void togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            pauseCamera.enabled = false;
            paused = false;
        }
        else
        {
            Time.timeScale = 0f;
            pauseCamera.enabled = true;
            paused = true;
        }
    }
}