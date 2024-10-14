using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private PlayerController m_playerController;
    
    [Header("Movement")]
    [SerializeField] private float m_playerSpeed = 2.0f;
    [SerializeField] private float m_playerRotationSpeed = 2.0f;
    
    [Header("Jumping")]
    [SerializeField] private float m_jumpHeight = 1.0f;
    [SerializeField] private float m_gravityValue = -9.81f;
    
    [Header("Unstuck")]
    [SerializeField] private float m_unstuckJumpForce = 1.0f;
    [SerializeField] private float m_unstuckBackForce = 1.0f;


    private Vector3 m_playerVelocity;
    private bool m_isGrounded = true;
    private Vector3 m_movementFromInput;
    private float m_movementSpeedMultiplier = 1f;
    private Transform m_cameraReference;

    public float NormalizedMovementSpeed => m_playerSpeed * (m_movementFromInput.magnitude / m_playerSpeed);
    public float MovementSpeed => m_playerSpeed * m_movementFromInput.magnitude;

    public float MovementSpeedMultiplier
    {
        get => m_movementSpeedMultiplier;
        set
        {
            if (value < 1f)
            {
                m_movementSpeedMultiplier = 1f;
                Debug.LogError("MovementSpeedMultiplier under 1");
            }
            m_movementSpeedMultiplier = value;
        }
    }

    public bool IsGrounded => m_isGrounded;

    private void Start()
    {
        m_cameraReference = m_playerController.m_cameraMovement.CameraReference;
    }

    void Update()
    {
        // Work around instead of disable this script and not affecting the player velocity
        // Need to Zero out player Velocity when stuck
        if (!m_playerController.m_playerAbilities.IsStuck)
            PlayerMove();
        ApplyGravity();
    }

    public void GetMovementValueFromInput(InputAction.CallbackContext movementVector)
    {
        Vector2 movementValue = movementVector.ReadValue<Vector2>();
        
        // This is if you are charging you want to keep the charge at maximum running speed or else it will feel weird
        // Keeps player running at max speed but still can turn
        m_movementFromInput = new Vector3(movementValue.x, 0, movementValue.y);
    }

    private void PlayerMove()
    {
        Vector3 rotationDirection = m_movementFromInput.x < 0 ? -m_cameraReference.right : m_cameraReference.right;

        if (m_movementFromInput != Vector3.zero)
        {
            // Quaternion nextRotation = Quaternion.RotateTowards(transform.rotation,
            //     Quaternion.LookRotation(rotationDirection),
            //     m_playerRotationSpeed * Mathf.Abs(m_movementFromInput.x));
            Quaternion nextRotation = Quaternion.LookRotation(m_movementFromInput) * 
                                      Quaternion.LookRotation(m_cameraReference.forward);
            transform.rotation = nextRotation;
        }

        m_characterController.Move(
            transform.forward * (m_movementFromInput.magnitude * 
                                 (Time.deltaTime * m_playerSpeed * m_movementSpeedMultiplier)));
    }

    private void ApplyGravity()
    {
        // Do this zero velocity before check if character controller is enabled because
        // else it saves its previous velocity when getting stuck
        if (m_isGrounded && m_playerVelocity.y < 0 || m_playerController.m_playerAbilities.IsStuck)
        {
            m_playerVelocity = Vector3.zero;
        }
        
        if (!m_characterController.enabled) return;

        m_playerVelocity.y += m_gravityValue * Time.deltaTime;
        m_characterController.Move(m_playerVelocity * Time.deltaTime);
    }
    
    // Called from unity event
    public void SetIsGrounded(bool isGrounded)
    {
        m_isGrounded = isGrounded;
    }

    // Called from player input event
    public void Jump()
    {
        if (!m_isGrounded) return;
        
        m_playerVelocity.y += Mathf.Sqrt(m_jumpHeight * -3.0f * m_gravityValue);
    }
    
    // Called from unstuck event on Player Abilities
    public void UnstuckJump()
    {
        m_playerVelocity.y += Mathf.Sqrt(m_unstuckJumpForce);
        m_playerVelocity += -transform.forward * Mathf.Sqrt(m_unstuckBackForce);
    }

}