using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour {

    // Use this for initialization
    void Start () {
	}
    /// <summary>
    /// Detects when the player has finished the level and notify the level manager. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            LevelManager.playerGoalReached.Invoke();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
