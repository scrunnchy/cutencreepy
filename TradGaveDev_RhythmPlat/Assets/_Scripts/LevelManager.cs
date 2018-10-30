using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public bool paused { get; private set; }

    public bool isReversed = false;

    Camera pauseCamera;
    Camera mainCam;
    AudioSource audio;

    private void Awake()
    {
        // ensure time is moving upon awake
        paused = false;
        Time.timeScale = 1f;
    }

    // Use this for initialization
    void Start()
    {
        //Get camera and disable 
        GameObject pauseObject = GameObject.Find("Pause Camera");
        if (pauseObject != null)
            pauseCamera = pauseObject.GetComponent<Camera>();
        pauseCamera.enabled = false;

        GameObject mainObject = GameObject.Find("Main Camera");
        mainCam = mainObject.GetComponent<Camera>();
        AudioSource audio = mainCam.GetComponent<AudioSource>();

        Checkpoint.CheckpointReverse.AddListener(flipCamera);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Toggles the pause state
    /// </summary>
    /// <returns></returns>
    public void togglePause()
    {
        // if paused, play
        if (Time.timeScale == 0f)
        {
            // play audio
            audio.Play();
            Time.timeScale = 1f;
            pauseCamera.enabled = false;
            paused = false;
        }
        // if playing, pause
        else
        {
            // pause audio
            audio.Pause();
            Time.timeScale = 0f;
            pauseCamera.enabled = true;
            paused = true;
        }
    }

    public void stallCamera()
    {

    }
    /// <summary>
    /// Flips the main camera 180 degrees
    /// </summary>
    public void flipCamera()
    {
        isReversed = !isReversed;

        Vector3 angle = new Vector3(0, 180, 0);
        flipCamera(angle);
    }


    /// <summary>
    /// Moves the given camera a given distance and angle
    /// </summary>
    /// <param name="angle">angle of rotation</param>
    private void flipCamera(Vector3 angle)
    {
        audio.pitch = -1;
        mainCam.transform.Rotate(angle);
    }
}