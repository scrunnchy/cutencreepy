using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This component, when attached to an object with children wil set all 
/// children to active or inactive based on the public booleans in the inspector. 
/// 
/// It also toggles this activity when it hears a reversal event. 
/// </summary>
public class PlatformReverser : MonoBehaviour {

    //Produces the behaviour that an enemy will no longer be a threat or visible after a reversal event
    public bool DisapearsAtReverse;
    //Produces the behaviour that an enemy will only become visible and a threat after a reversal event
    public bool AppearsAtReverse;

    private GameObject[] pieces;

    // Use this for initialization
    void Start () {
        //Register for reversal.
        Checkpoint.CheckpointReverse.AddListener(ChangeAppearance);

        pieces = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            pieces[i] = transform.GetChild(i).gameObject;
        }

        if (AppearsAtReverse)
        {
            foreach(GameObject go in pieces)
            {
                go.SetActive(false);
            }
        }
    }

    /// <summary>
    /// activates/deactivates the children of this game object on reversal.
    /// </summary>
    private void ChangeAppearance()
    {
        //appear if necessary
        if (AppearsAtReverse)
        {
            foreach (GameObject go in pieces)
            {
                go.SetActive(true);
            }
        }
        //dissapear if necessary
        if (DisapearsAtReverse)
        {
            foreach (GameObject go in pieces)
            {
                go.SetActive(false);
            }
        }
    }
	
}
