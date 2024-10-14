using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private Transform m_cameraReferenceTransform;
    
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
    
    [Space]
    [Header("Rotate Around Player")]
    [SerializeField] private float m_rotateSpeed;

    private Vector3 m_lookFromInput;

    // To get camera direction, for moving in the cameras direction
    public Transform CameraReference => m_cameraReferenceTransform;

    void LateUpdate()
    {
        FollowTarget();
        LookAtTarget();
        RotateAroundPlayerFromInput();
    }
    
    private void FollowTarget()
    {
        Vector3 cameraPosition = m_cameraTransform.position;
        Vector3 targetActualPosition = m_targetToFollow.position;
        Vector3 followDirection = (transform.position - Vector3.up * m_cameraOffsetPosFromTarget.y -
                                   targetActualPosition).normalized;
        
        Vector3 targetToFollowPosition = followDirection * m_cameraOffsetPosFromTarget.x
                                         + targetActualPosition;
        
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
    
    // Called from Input Event on Player
    public void GetLookValueFromInput(InputAction.CallbackContext movementVector)
    {
        Vector2 movementValue = movementVector.ReadValue<Vector2>();
        m_lookFromInput = new Vector3(movementValue.x, 0, movementValue.y);
    }

    private void RotateAroundPlayerFromInput()
    {
        transform.RotateAround(m_targetToLookAt.position, Vector3.up,
            m_rotateSpeed * m_lookFromInput.x * Time.deltaTime);
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
