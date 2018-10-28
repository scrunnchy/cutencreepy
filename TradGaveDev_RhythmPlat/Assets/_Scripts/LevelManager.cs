using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public bool paused { get; private set; }
    public float cameraDistanceFromPlayer = 50f;
    public bool isReversed = false;

    Camera pauseCamera;

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
        Camera gameCamera;
        AudioSource audio;
        GameObject gameObject = GameObject.Find("Main Camera");
        if (gameObject != null)
        {
            gameCamera = gameObject.GetComponent<Camera>();
            audio = gameCamera.GetComponent<AudioSource>();
        }
        else
            audio = new AudioSource();

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

    public void flipCamera()
    {
        isReversed = !isReversed;
        Camera gameCamera;
        AudioSource audio;
        GameObject gameObject = GameObject.Find("Main Camera");
        if (gameObject != null)
        {
            gameCamera = gameObject.GetComponent<Camera>();
            Vector3 angle = new Vector3(0, 180, 0);
            flipCamera(gameCamera, angle);
            
            audio = gameCamera.GetComponent<AudioSource>();
            audio.pitch = -1;
            
            //checkpoint.renderer.isVisible
        }
    }

    /// <summary>
    /// Moves the given camera a given distance and angle
    /// </summary>
    /// <param name="camera">camera</param>
    /// <param name="distance">added distance</param>
    /// <param name="angle">angle of rotation</param>
    private void flipCamera(Camera camera, Vector3 angle)
    {
        PlatformerCameraFollow follower = camera.GetComponent<PlatformerCameraFollow>();
        camera.transform.Rotate(angle);
    }
}