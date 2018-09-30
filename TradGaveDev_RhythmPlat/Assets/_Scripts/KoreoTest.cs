using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class KoreoTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Koreographer.Instance.RegisterForEvents("eventIDHere", activateThisMethod);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void activateThisMethod (KoreographyEvent evt)
    {

    }
}
