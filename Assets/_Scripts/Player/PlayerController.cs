
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform m_chickenTransform;
    
    public PlayerMovement m_playerMovement;
    public PlayerAbilities m_playerAbilities;
    public AnimationController m_animationController;
    public RagdollController m_ragdollController;

    public bool IsRagdoll;

    private void Start()
    {
        
    }

    // Called from input for debug reaseons, will not be like that later
    public void SetRagdoll(bool willRagdoll)
    {
        IsRagdoll = willRagdoll;
        
        // Turn off the animator or else it will not ragdoll
        m_animationController.Animator.enabled = !willRagdoll;
        m_ragdollController.RagdollPlayer(willRagdoll);
        
        if (willRagdoll)
        {
            m_ragdollController.MainRigidBody.transform.parent = null;
        }
        else
        {
            m_chickenTransform.position = m_ragdollController.MainRigidBody.transform.position;
            m_ragdollController.MainRigidBody.transform.parent = m_chickenTransform;
            m_ragdollController.MainRigidBody.transform.localPosition = Vector3.zero;
            m_ragdollController.MainRigidBody.transform.localRotation = m_ragdollController.MainBodyLocalStartRotation;
        }
    }

}
