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

    //define all event types from other classes
    public static UnityEvent playerGoalReached;
    public static UnityEvent enemyPlayerCollision;
    public static UnityEvent CheckpointCollision;
    public static UnityEvent CheckpointReverse;

    public Koreography forwardTrack;
    public Koreography reverseTrack;

    AudioSource audioCom = null;

    Camera pauseCamera;
    Camera mainCam;
    private AudioSource audio;
    UIManager UIM;

    //initialize all UnityEvents here
    private void Awake()
    {
        playerGoalReached = new UnityEvent();

        enemyPlayerCollision = new UnityEvent();

        CheckpointCollision = new UnityEvent();

        CheckpointReverse = new UnityEvent();

        // ensure time is moving upon awake
        paused = false;
        Time.timeScale = 1f;
    }

    // Use this for initialization
    void Start()
    {
        //Get UI manager
        UIM = GameObject.Find("UIManager").GetComponent<UIManager>();
        UIM.pauseAudio.Stop();
        //Get camera and disable 
        mainCam = Camera.main;
        GameObject pauseObject = GameObject.Find("Pause Camera");
        if (pauseObject != null)
            pauseCamera = pauseObject.GetComponent<Camera>();
        pauseCamera.enabled = false;

        audio = mainCam.GetComponent<AudioSource>();

        CheckpointReverse.AddListener(flipCamera);
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
            UIM.pauseAudio.Pause();
            Time.timeScale = 1f;
            pauseCamera.enabled = false;
            paused = false;
        }
        // if playing, pause
        else
        {
            // pause audio
            audio.Pause();
            UIM.pauseAudio.Play();
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