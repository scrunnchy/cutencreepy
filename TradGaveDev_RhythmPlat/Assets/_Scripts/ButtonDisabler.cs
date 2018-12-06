using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabler : MonoBehaviour {

    public void disable()
    {
        gameObject.GetComponent<Button>().interactable = false;
    }
}
