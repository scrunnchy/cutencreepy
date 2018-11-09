﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public AudioSource pauseAudio;

    LevelManager LM;

    Button exitButton;
    
    // Use this for initialization
    void Start()
    {
        pauseAudio = GetComponent<AudioSource>();
        exitButton = GetButton("ExitButton");
        exitButton.onClick.AddListener(onExit);

        LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();

    }

    // Update is called once per frame
    void Update()
    {
        checkForKeyStroke();
    }

    /// <summary>
    /// Exits game
    /// </summary>
    public void onExit()
    {
        Application.Quit();
    }

    #region helpers 
    /// <summary>
    /// Retrieves a button from the scene
    /// </summary>
    /// <param name="name">button name</param>
    /// <returns></returns>
    private Button GetButton(string name)
    {
        GameObject gameObject = GameObject.Find(name);
        if (gameObject != null)
            return gameObject.GetComponent<Button>();
        return null;
    }
    
    private void checkForKeyStroke()
    {
        if (Input.GetButtonDown("pauseButton"))
            LM.togglePause();
    }
}

#endregion