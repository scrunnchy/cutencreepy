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

        //Test for flipCamera
        if (Input.GetButtonDown("Horizontal"))
        {
            Camera gameCamera;
            GameObject gameObject = GameObject.Find("Main Camera");
            if (gameObject != null)
            {
                gameCamera = gameObject.GetComponent<Camera>();
                Vector3 angle = new Vector3(0, 180, 0);
                Vector3 pos = new Vector3(0, 0, 100);
                LM.flipCamera(gameCamera, pos, angle);
            }
        }
    }
}

#endregion