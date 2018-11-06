using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using SonicBloom.Koreo;

public class LevelManager : MonoBehaviour
{

    public bool paused { get; private set; }

    public bool isReversed = false;


    public Koreography forwardTrack;
    public Koreography reverseTrack;

    AudioSource audioCom = null;

    Camera pauseCamera;
    Camera mainCam;
    private AudioSource audio;

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
        mainCam = Camera.main;
        GameObject pauseObject = GameObject.Find("Pause Camera");
        if (pauseObject != null)
            pauseCamera = pauseObject.GetComponent<Camera>();
        pauseCamera.enabled = false;

        audio = mainCam.GetComponent<AudioSource>();

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

    /// <summary>
    /// Flips the main camera 180 degrees
    /// </summary>
    public void flipCamera()
    {
        isReversed = !isReversed;
        _flipCamera();
    }


    /// <summary>
    /// Moves the given camera a given angle and reverses audio
    /// </summary>
    /// <param name="angle">angle of rotation</param>
    private void _flipCamera()
    {
        Vector3 angle = new Vector3(0, 180, 0);
        mainCam.transform.Rotate(angle);
    }
}