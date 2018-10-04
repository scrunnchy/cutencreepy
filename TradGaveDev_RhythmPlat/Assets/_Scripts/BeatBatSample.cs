using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SonicBloom.Koreo;

public class BeatBatSample : MonoBehaviour {

    public Transform[] waypoints;
    NavMeshAgent agent;
    Animator animator;
    int index; // the current waypoint index in the waypoints array
    float speed, agentSpeed; // current agent speed and NavMeshAgent component speed

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agentSpeed = agent.speed;
        animator = GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
        Koreographer.Instance.RegisterForEvents("SingleBeatTrack", animateBat);
    }


	// Update is called once per frame
	void Update () {
        animator.SetFloat("Speed", agent.velocity.magnitude);

    }

    void animateBat(KoreographyEvent evt)
    {
        //Debug.Log("Code reached");
        index = index == waypoints.Length - 1 ? 0 : index + 1;
        agent.destination = waypoints[index].position;
        Debug.Log(agent.destination);
        agent.speed = agentSpeed;
    }
}
