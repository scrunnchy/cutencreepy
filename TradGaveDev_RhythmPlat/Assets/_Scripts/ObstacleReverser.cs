using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleReverser : MonoBehaviour {

    //Produces the behaviour that a falling rock will no longer be a threat or visible during reversals
    public bool DisapearsAtReverse;
    //Produces the behaviour that a falling rock will only become visible and a threat during reversals
    public bool AppearsAtReverse;

    //store the reversal state shown in the level manager
    private GameObject[] pieces;

    // Use this for initialization
    void Start () {
        //Register for reversal.
        LevelManager.CheckpointReverse.AddListener(ChangeAppearanceOnReverse);

        pieces = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            pieces[i] = transform.GetChild(i).gameObject;
        }

        if (AppearsAtReverse)
        {
            foreach (GameObject go in pieces)
            {
                go.SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// triggers on reversal events
    /// sets the active state of children to reflect reversal settings.
    /// </summary>
    private void ChangeAppearanceOnReverse()
    {
        //appear if invisable and dissapear if not
        if (AppearsAtReverse || DisapearsAtReverse)
        {
            foreach (GameObject go in pieces)
            {
                //swap to active or innactive based on current state
                if (go.activeSelf)
                {
                    go.SetActive(false);
                }
                else
                {
                    go.SetActive(true);
                }
            }
        }
    }
}
