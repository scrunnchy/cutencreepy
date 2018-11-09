using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour {

    //define event to be used when this is reached
    public static UnityEvent playerGoalReached;

    private void Awake()
    {
        //make sure event is initialized
        if (playerGoalReached == null)
        {
            playerGoalReached = new UnityEvent();
        }
    }
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
            playerGoalReached.Invoke();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
