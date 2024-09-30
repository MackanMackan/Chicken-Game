using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator m_playerAnimator;
    [SerializeField] private PlayerMovement m_playerMovement;
    [SerializeField] private float m_movementAnimationSpeedMultiplier;

    private int m_jumpTriggerHash;
    private int m_movementSpeedHash;
    private int m_normalizedMovementSpeedHash;
    private int m_isGroundedHash;
    void Start()
    {
        m_jumpTriggerHash = Animator.StringToHash("Jump");
        m_movementSpeedHash = Animator.StringToHash("MovementSpeed");
        m_normalizedMovementSpeedHash = Animator.StringToHash("NormalizedMovementSpeed");
        m_isGroundedHash = Animator.StringToHash("IsGrounded");
    }
    
    void Update()
    {
        // For Walk/Run Animation Speed
        m_playerAnimator.SetFloat(m_movementSpeedHash, m_playerMovement.MovementSpeed * m_movementAnimationSpeedMultiplier);
        // For Walk/Run State Control
        m_playerAnimator.SetFloat(m_normalizedMovementSpeedHash, m_playerMovement.NormalizedMovementSpeed);
        
        // For Idle Animation
        m_playerAnimator.SetBool(m_isGroundedHash, m_playerMovement.IsGrounded);
    }

    public void DoJumpAnimation()
    {
        m_playerAnimator.SetTrigger(m_jumpTriggerHash);
    }
}
