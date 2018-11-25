using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// This component, when attached to an object with children wil set all 
/// children to active or inactive based on the public booleans in the inspector. 
/// 
/// It also toggles this activity when it hears a reversal event. 
/// </summary>
public class PlatformReverser : MonoBehaviour {

    //Produces the behaviour that a platform will no longer be a threat or visible during reversals
    public bool DisapearsAtReverse;
    //Produces the behaviour that a platform will only become visible and a threat during reversals
    public bool AppearsAtReverse;

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
            foreach(GameObject go in pieces)
            {
                //disable all box colliders or sprite renderers
                SpriteRenderer tempS = go.GetComponent<SpriteRenderer>();
                if (tempS != null)
                {
                    tempS.enabled = false;
                }
                BoxCollider tempB = go.GetComponent<BoxCollider>();
                if (tempB != null)
                {
                    tempB.enabled = false;
                }
            }
        }
    }

    /// <summary>
    /// activates/deactivates the children of this game object on reversal.
    /// </summary>
    private void ChangeAppearanceOnReverse()
    {
        //appear if invisable and dissapear if not
        if (AppearsAtReverse || DisapearsAtReverse)
        {
            foreach (GameObject go in pieces)
            {
                //swap relevant components to active or innactive based on current state
                SpriteRenderer tempS = go.GetComponent<SpriteRenderer>();
                if (tempS != null)
                {
                    tempS.enabled = !tempS.enabled;
                }
                BoxCollider tempB = go.GetComponent<BoxCollider>();
                if (tempB != null)
                {
                    tempB.enabled = !tempB.enabled;
                }
            }
        }
    }
	
}
