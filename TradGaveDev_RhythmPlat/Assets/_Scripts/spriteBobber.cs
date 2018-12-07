using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteBobber : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
    }
	
	// bob this object up and down rhythmically
	void Update () {
        if (Time.timeScale != 0)
        {
            transform.Translate(0, (float)System.Math.Sin(Time.time * 3) / 120, 0);
        }
    }
}
