using System;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private GameObject m_mainRigidBody;
    [SerializeField] private List<Rigidbody> m_rigidbodies = new List<Rigidbody>();
    [SerializeField] private List<Collider> m_colliders = new List<Collider>();

    private Quaternion m_mainBodyLocalStartRotation;

    public Quaternion MainBodyLocalStartRotation => m_mainBodyLocalStartRotation;

    public GameObject MainRigidBody => m_mainRigidBody;

    private void Start()
    {
        m_mainBodyLocalStartRotation = m_mainRigidBody.transform.rotation;
    }

    public void RagdollPlayer(bool isRagdoll)
    {
        foreach (Rigidbody rigidbody in m_rigidbodies)
        {
            rigidbody.isKinematic = !isRagdoll;
        }

        foreach (Collider collider in m_colliders)
        {
            collider.isTrigger = !isRagdoll;
        }
    }
}
