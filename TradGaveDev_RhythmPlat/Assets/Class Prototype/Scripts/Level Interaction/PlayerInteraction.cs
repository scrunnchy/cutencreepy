using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour {

    public string playerTag = "Player";

    public UnityEvent playerInteractionEvent;

    public bool continuousInteraction = false;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInteractionEvent.Invoke();
        }
    }

    public void OnTriggerStay (Collider other)
    {
        if (continuousInteraction && other.CompareTag(playerTag))
        {
            playerInteractionEvent.Invoke();
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            playerInteractionEvent.Invoke();
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if (continuousInteraction && other.gameObject.CompareTag(playerTag))
        {
            playerInteractionEvent.Invoke();
        }
    }
}
