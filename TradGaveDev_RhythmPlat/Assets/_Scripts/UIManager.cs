using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public LevelManager LM;

    Button exitButton;

    // Use this for initialization
    void Start()
    {
        exitButton = GetButton("ExitButton");
        exitButton.onClick.AddListener(onExit);

    }

    // Update is called once per frame
    void Update()
    {
        checkForKeyStroke();
    }

    public void onExit()
    {
        Application.Quit();
    }

    #region helpers 
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