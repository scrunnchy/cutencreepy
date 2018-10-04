using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCameraFollow : MonoBehaviour {

    public Transform followTransform;
    public bool useFixedUpdate;
    public float followSpeed = 8f;

    [Space(12)]

    public bool lookAhead = false;
    public float lookAheadAmount = 2f;
    public float lookAheadSpeed = 2f;

    [Space(12)]

    public float startDelay = 0.5f;


    private bool _canFollow;

    private Vector3 _zOffset;
    private Vector3 _target;

    private Vector3 _lookOffset;

    void Start () 
    {
        _zOffset.z = this.transform.position.z - followTransform.position.z;     

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
        _target = followTransform.position;

        if (lookAhead)
        {
            _lookOffset = Vector3.Lerp(_lookOffset, (followTransform.forward * lookAheadAmount), Time.deltaTime * lookAheadSpeed);
            _target += _lookOffset;
        }

        _target += _zOffset;

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
