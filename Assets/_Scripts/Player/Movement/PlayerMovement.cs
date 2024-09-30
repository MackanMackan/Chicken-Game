using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController m_playerController;
    [SerializeField] private float m_playerSpeed = 2.0f;
    [SerializeField] private float m_playerRotationSpeed = 2.0f;
    [SerializeField] private float m_jumpHeight = 1.0f;
    [SerializeField] private float m_gravityValue = -9.81f;

    private Vector3 m_playerVelocity;
    private bool m_isGrounded = true;
    private Vector3 m_movement;

    public float NormalizedMovementSpeed => m_playerSpeed * (m_movement.magnitude / m_playerSpeed);
    public float MovementSpeed => m_playerSpeed * m_movement.magnitude;
    public bool IsGrounded => m_isGrounded;

    void Update()
    {
        PlayerMove();
        ApplyGravity();
    }

    public void GetMovementValueFromInput(InputAction.CallbackContext movementVector)
    {
        Vector2 movementValue = movementVector.ReadValue<Vector2>();
        m_movement = new Vector3(movementValue.x, 0, movementValue.y);
    }

    private void PlayerMove()
    {
        Vector3 rotationDirection = m_movement.x < 0 ? -transform.right : transform.right;

        if (m_movement != Vector3.zero)
        {
            Quaternion nextRotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(rotationDirection),
                m_playerRotationSpeed * Mathf.Abs(m_movement.x));

            transform.rotation = nextRotation;
        }

        if (m_movement.z < 0)
            return;

        m_playerController.Move(
            gameObject.transform.forward * (m_movement.magnitude * (Time.deltaTime * m_playerSpeed)));
    }

    public void SetIsGrounded(bool isGrounded)
    {
        m_isGrounded = isGrounded;
    }

    public void Jump()
    {
        if (!m_isGrounded) return;

        m_playerVelocity.y += Mathf.Sqrt(m_jumpHeight * -3.0f * m_gravityValue);
    }

    private void ApplyGravity()
    {
        if (m_isGrounded && m_playerVelocity.y < 0)
        {
            m_playerVelocity.y = 0f;
        }

        m_playerVelocity.y += m_gravityValue * Time.deltaTime;
        m_playerController.Move(m_playerVelocity * Time.deltaTime);
    }
}