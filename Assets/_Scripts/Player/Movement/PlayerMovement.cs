using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private float m_playerSpeed = 2.0f;
    [SerializeField] private float m_playerRotationSpeed = 2.0f;
    [SerializeField] private float m_jumpHeight = 1.0f;
    [SerializeField] private float m_gravityValue = -9.81f;

    private Vector3 m_playerVelocity;
    private bool m_isGrounded = true;
    private Vector3 m_movementFromInput;
    private float m_movementSpeedMultiplier = 1f;

    public float NormalizedMovementSpeed => m_playerSpeed * (m_movementFromInput.magnitude / m_playerSpeed);
    public float MovementSpeed => m_playerSpeed * m_movementFromInput.magnitude;

    public float MovementSpeedMultiplier
    {
        get => m_movementSpeedMultiplier;
        set
        {
            if (value <= 1f)
            {
                m_movementSpeedMultiplier = 1f;
                Debug.LogError("MovementSpeedMultiplier under 1");
            }
            m_movementSpeedMultiplier = value;
        }
    }

    public bool IsGrounded => m_isGrounded;

    void Update()
    {
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
        Vector3 rotationDirection = m_movementFromInput.x < 0 ? -transform.right : transform.right;

        if (m_movementFromInput != Vector3.zero)
        {
            Quaternion nextRotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(rotationDirection),
                m_playerRotationSpeed * Mathf.Abs(m_movementFromInput.x));

            transform.rotation = nextRotation;
        }

        if (m_movementFromInput.z < 0)
            return;

        m_characterController.Move(
            gameObject.transform.forward * (m_movementFromInput.magnitude * 
                                            (Time.deltaTime * m_playerSpeed * m_movementSpeedMultiplier)));
    }

    // Called from unity event
    public void SetIsGrounded(bool isGrounded)
    {
        m_isGrounded = isGrounded;
    }

    // Called from unity event
    public void Jump()
    {
        if (!m_isGrounded) return;
        
        m_playerVelocity.y += Mathf.Sqrt(m_jumpHeight * -3.0f * m_gravityValue);
    }

    private void ApplyGravity()
    {
        if (m_isGrounded && m_playerVelocity.y < 0 || m_playerController.m_playerAbilities.IsStuck)
        {
            m_playerVelocity.y = 0f;
        }

        m_playerVelocity.y += m_gravityValue * Time.deltaTime;
        m_characterController.Move(m_playerVelocity * Time.deltaTime);
    }
}