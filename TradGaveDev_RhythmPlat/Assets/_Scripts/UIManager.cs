using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public AudioSource pauseAudio { get; private set; }

    [SerializeField]
    public LevelManager LM;

    Button exitButton;

    private void Awake()
    {
        pauseAudio = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        exitButton = GetButton("ExitButton");
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(onExit);
        }
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
        if (Input.GetButtonDown("pauseButton") && LM != null)
            LM.togglePause();
    }
}

#endregion