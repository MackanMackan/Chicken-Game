using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCollisionEventForwarder : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider> OnTriggerEntered;
    [SerializeField] private UnityEvent<Collider> OnTriggerExited;
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEntered?.Invoke(other);
        Debug.Log("Entered");
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExited?.Invoke(other);
        Debug.Log("Exit");
    }
}
