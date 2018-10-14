using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public bool paused { get; private set; }
    public int cameraDistanceFromPlayer;

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
        // if paused, play
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            pauseCamera.enabled = false;
            paused = false;
        }
        // if playing, pause
        else
        {
            Time.timeScale = 0f;
            pauseCamera.enabled = true;
            paused = true;
        }
    }

    public void flipCamera()
    {
        Camera gameCamera;
        GameObject gameObject = GameObject.Find("Main Camera");
        if (gameObject != null)
        {
            gameCamera = gameObject.GetComponent<Camera>();
            Vector3 angle = new Vector3(0, 180, 0);
            Vector3 pos = new Vector3(0, 0, cameraDistanceFromPlayer);
            flipCamera(gameCamera, pos, angle);
        }
    }

    /// <summary>
    /// Moves the given camera a given distance and angle
    /// </summary>
    /// <param name="camera">camera</param>
    /// <param name="distance">added distance</param>
    /// <param name="angle">angle of rotation</param>
    private void flipCamera(Camera camera, Vector3 distance, Vector3 angle)
    {
        camera.transform.Translate(distance);
        camera.transform.Rotate(angle);
    }
}