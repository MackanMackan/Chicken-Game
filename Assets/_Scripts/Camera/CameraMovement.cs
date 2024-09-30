using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform m_cameraTransform;
    
    [Space]
    [Header("Follow")]
    [SerializeField] private Transform m_targetToFollow;
    [SerializeField] private Vector2 m_cameraOffsetPosFromTarget;
    [SerializeField] private float m_followSpeed = 1;
    
    [Space]
    [Header("Look At")]
    [SerializeField] private Transform m_targetToLookAt;
    [SerializeField] private Vector3 m_cameraOffsetLookAtTarget;
    [SerializeField] private float m_lookAtSmoothness;

    void LateUpdate()
    {
        FollowTarget();
        LookAtTarget();
    }
    
    private void FollowTarget()
    {
        Vector3 cameraPosition = m_cameraTransform.position;
        
        Vector3 targetToFollowPosition = -m_targetToFollow.forward * 
            m_cameraOffsetPosFromTarget.x + m_targetToFollow.position;
        
        Vector3 targetPosition = new Vector3(targetToFollowPosition.x,
            targetToFollowPosition.y + m_cameraOffsetPosFromTarget.y,
            targetToFollowPosition.z);
        
        Vector3 newPosition = cameraPosition + (targetPosition - cameraPosition ) * (0.1f * m_followSpeed);
        m_cameraTransform.position = newPosition;
    }
    
    private void LookAtTarget()
    {
        Vector3 lookAtPosition = m_targetToLookAt.position + m_cameraOffsetLookAtTarget;
        Vector3 normalLookDirection = (lookAtPosition - transform.position).normalized;
        Quaternion rotationToMoveTowards = Quaternion.LookRotation(normalLookDirection);
        
        Quaternion nextRotation =
            Quaternion.RotateTowards(m_cameraTransform.rotation, rotationToMoveTowards, m_lookAtSmoothness);

        m_cameraTransform.rotation = nextRotation;
    }
#if UNITY_EDITOR
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_targetToFollow.position, m_targetToFollow.position + -m_targetToFollow.forward);
        
        Gizmos.color = Color.blue;
        Vector3 targetToFollowPosition = -m_targetToFollow.forward * m_cameraOffsetPosFromTarget.x  + m_targetToFollow.position;
        
        Vector3 targetPosition = new Vector3(targetToFollowPosition.x,
            targetToFollowPosition.y + m_cameraOffsetPosFromTarget.y,
            targetToFollowPosition.z);
        Gizmos.DrawLine(targetToFollowPosition, targetPosition);
    }
#endif
}