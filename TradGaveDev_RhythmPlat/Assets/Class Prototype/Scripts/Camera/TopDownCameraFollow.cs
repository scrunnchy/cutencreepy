using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraFollow : MonoBehaviour {

    public Transform followTransform;
    public bool useFixedUpdate;
    public float followSpeed = 8f;

    [Space(12)]

    public float startDelay = 0.5f;


    private bool _canFollow;

    private Vector3 _Offset;
    private Vector3 _target;

    private Vector3 _lookOffset;

    void Start () 
    {
        _Offset = this.transform.position - followTransform.position;     

        if (startDelay != 0f) 
        {
            StartCoroutine(StartFollowDelay());
        }
        else 
        {
            _canFollow = true;
        }
    }
    
    void Update()
    {
        _target = followTransform.position + _Offset;

        if (!useFixedUpdate && _canFollow)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, _target, Time.deltaTime * followSpeed);
        }
    }

    void FixedUpdate () 
    {
        if (useFixedUpdate && _canFollow)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, _target, Time.fixedDeltaTime * followSpeed);
        }
    }

    IEnumerator StartFollowDelay ()
    {
        yield return new WaitForSeconds(startDelay);

        _canFollow = true;
    }

}
