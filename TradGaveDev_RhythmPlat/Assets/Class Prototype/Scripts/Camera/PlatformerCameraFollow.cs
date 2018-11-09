using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCameraFollow : MonoBehaviour
{

    public Transform followTransform;
    public bool useFixedUpdate;
    public float followSpeed = 8f;
    public bool isFollowing = true;


    [Space(12)]

    public bool lookAhead = false;
    public float lookAheadAmount = 2f;
    public float lookAheadSpeed = 2f;

    [Space(12)]

    public float startDelay = 0.5f;
    public float xOffset = 5f;
    public float yOffset = 2f;
    public float cameraDistanceFromPlayer = 50f;

    [Space(12)]

    public LevelManager LevelManager;

    private bool _canFollow;

    private Vector3 _zOffset;
    private Vector3 _target;

    private Vector3 _lookOffset;


    void Start()
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

        LevelManager.CheckpointReverse.AddListener(changeZ);
    }

    void Update()
    {
        if (isFollowing)
        {
            _target = followTransform.position;
            _target.y += yOffset;
            if (LevelManager.isReversed)
                _target.x -= xOffset;
            else
                _target.x += xOffset;

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
    }

    void FixedUpdate()
    {
        if (useFixedUpdate && _canFollow)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, _target, Time.fixedDeltaTime * followSpeed);
        }
    }

    IEnumerator StartFollowDelay()
    {
        yield return new WaitForSeconds(startDelay);

        _canFollow = true;
    }

    private void changeZ()
    {
        if (LevelManager.isReversed)
        {
            _zOffset.z -= cameraDistanceFromPlayer;
            Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - cameraDistanceFromPlayer);
            this.transform.position = pos;
        }
        else
        {
            _zOffset.z += cameraDistanceFromPlayer;
            Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + cameraDistanceFromPlayer);
            this.transform.position = pos;
        }
        isFollowing = true;
    }
}
