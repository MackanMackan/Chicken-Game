using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> m_rigidbodies = new List<Rigidbody>();
    [SerializeField] private List<Collider> m_colliders = new List<Collider>();

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
