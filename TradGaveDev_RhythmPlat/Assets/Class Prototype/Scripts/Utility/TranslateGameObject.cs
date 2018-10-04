using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateGameObject : MonoBehaviour {

    public Vector3 worldPosition;

    public void MoveToPosition ()
    {
        this.gameObject.transform.position = worldPosition;
    }
}
