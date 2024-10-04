using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCollisionEventForwarder : MonoBehaviour
{
    // TODO: Set up a class that helps with camparing tags and layers with this attribute
    [SerializeField] private bool m_useTag;
    [TagSelector] public string[] TagFilterList = new string[] {};
    
    [Space]
    
    [SerializeField] private bool m_useLayer;
    [SerializeField] private LayerMask m_layersToCheck = -1 << 0;
    
    public UnityEvent<Collider> OnTriggerEntered;
    public UnityEvent<Collider> OnTriggerExited;
    
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEntered?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExited?.Invoke(other);
    }

    private void CheckTag(string[] tags)
    {
        
    }
}
