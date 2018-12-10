using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPageRevealer : MonoBehaviour
{

    Camera infoCamera;
    Canvas infoCanvas;
    GameObject mainButtonPanel;
    // Use this for initialization
    void Start()
    {
        infoCamera = GameObject.Find("InfoCamera").GetComponent<Camera>();
        infoCamera.enabled = false;
        infoCanvas = GameObject.Find("InfoCanvas").GetComponent<Canvas>();
        infoCanvas.enabled = false;
        mainButtonPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("pauseButton"))
            closeScreen();
    }

    public void loadScreen()
    {
        mainButtonPanel.SetActive(false);
        infoCamera.enabled = true;
        infoCanvas.enabled = true;
    }

    public void closeScreen()
    {
        infoCamera.enabled = false;
        infoCanvas.enabled = false;
        mainButtonPanel.SetActive(true);
    }
}
