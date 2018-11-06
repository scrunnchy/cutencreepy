using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class PlatformBlink : MonoBehaviour {
    public float beatTrackRegisterNumber;

    public float blinkDelay;

    private GameObject[] pieces;

    // Use this for initialization
    void Start () {
        if (beatTrackRegisterNumber == .5)
        {
            Koreographer.Instance.RegisterForEvents("HalfBeatTrack", togglePlatform);
        }
        //Register for reversal.
        else if (beatTrackRegisterNumber == 1)
        {
            Koreographer.Instance.RegisterForEvents("SingleBeatTrack", togglePlatform);
        }
        else if (beatTrackRegisterNumber == 2)
        {
            Koreographer.Instance.RegisterForEvents("EveryOtherBeatTrack", togglePlatform);
        }

        pieces = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            pieces[i] = transform.GetChild(i).gameObject;
        }


    }
	void togglePlatform(KoreographyEvent evt)
    {
            StartCoroutine("DelayBlink");
    }
    IEnumerator DelayBlink()
    {
        yield return new WaitForSeconds(blinkDelay);
        foreach (GameObject go in pieces)
        {
            if (go.activeSelf == true)
                go.SetActive(false);
            else
            {
                go.SetActive(true);
            }
        }
    }

	
}
