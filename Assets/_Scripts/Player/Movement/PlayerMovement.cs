using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController m_playerController;
    [SerializeField] private float m_playerSpeed = 2.0f;
    [SerializeField]  private float m_jumpHeight = 1.0f;
    [SerializeField] private float m_gravityValue = -9.81f;
    
    private Vector3 m_playerVelocity;
    private bool m_groundedPlayer;

    void Update()
    {
        
    }

    public void MovePlayer(InputAction.CallbackContext movementVector)
    {
        m_groundedPlayer = m_playerController.isGrounded;
        if (m_groundedPlayer && m_playerVelocity.y < 0)
        {
            m_playerVelocity.y = 0f;
        }

        Vector2 movementValue = movementVector.ReadValue<Vector2>();
        Vector3 move = new Vector3(movementValue.x, 0, movementValue.y);
        m_playerController.Move(move * Time.deltaTime * m_playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
    }

    public void Jump()
    {
        if (m_groundedPlayer)
        {
            m_playerVelocity.y += Mathf.Sqrt(m_jumpHeight * -3.0f * m_gravityValue);
        }
    }
    
    private void ApplyGravity()
    {
        m_playerVelocity.y += m_gravityValue * Time.deltaTime;
        m_playerController.Move(m_playerVelocity * Time.deltaTime);
    }
}
