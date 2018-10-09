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
    Button continueButton;

    private void Awake()
    {
        paused = false;
    }

    // Use this for initialization
    void Start()
    {
        //Get button and add a listener 
        GameObject continueObject = GameObject.Find("ContinueButton");
        if (continueObject != null)
            continueButton = continueObject.GetComponent<Button>();
        continueButton.onClick.AddListener(onContinue);

        //Add listeners for collision events
        conductor.AddListener(enemyPlayerCollision);
        conductor.AddListener(checkpointCollision);
        conductor.AddListener(composerEvent);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void onContinue()
    {
        if (paused)
            togglePause();
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
            paused = false;
        }
        else
        {
            Time.timeScale = 0f;
            paused = true;
        }
    }
}