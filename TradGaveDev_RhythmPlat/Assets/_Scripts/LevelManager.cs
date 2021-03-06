﻿using System;
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
    public static UnityEvent enemyPlayerEndCollision;

    Camera pauseCamera;
    Camera mainCam;
    Canvas pauseCanvas;
    Button playButton;
    private AudioSource audio;
    UIManager UIM;

    //initialize all UnityEvents here
    private void Awake()
    {
        playerGoalReached = new UnityEvent();

        enemyPlayerCollision = new UnityEvent();

        CheckpointCollision = new UnityEvent();

        CheckpointReverse = new UnityEvent();

        enemyPlayerEndCollision = new UnityEvent();

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
        if (GameObject.Find("PauseCanvas") != null)
        {
            pauseCanvas = GameObject.Find("PauseCanvas").GetComponent<Canvas>();
        }
        if (GameObject.Find("PlayButton") != null)
        {
            playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        }
        if (pauseObject != null)
        {
            pauseCamera = pauseObject.GetComponent<Camera>();
            pauseCamera.enabled = false;
        }
        if (pauseCanvas != null)
            pauseCanvas.enabled = false;

        audio = mainCam.GetComponent<AudioSource>();
        CheckpointReverse.AddListener(reverseLevel);
    }

    private void reverseLevel()
    {
        isReversed = !isReversed;
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
        if (pauseCamera != null)
            // if paused, play
            if (Time.timeScale == 0f)
            {
                // play audio
                audio.Play();
                UIM.pauseAudio.Pause();
                Time.timeScale = 1f;
                pauseCamera.enabled = false;
                pauseCanvas.enabled = false;
                playButton.interactable = false;
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
                pauseCanvas.enabled = true;
                playButton.interactable = true;
                paused = true;
            }
    }
}