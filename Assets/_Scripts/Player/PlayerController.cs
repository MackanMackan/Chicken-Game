
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //TODO: Make it so that the character controller ends up on the body when it has rogdolled and will stand again
    public PlayerMovement m_playerMovement;
    public PlayerAbilities m_playerAbilities;
    public AnimationController m_animationController;
    public RagdollController m_ragdollController;

    public bool IsRagdoll = false;

    // Called from input for debug reaseons, will not be like that later
    public void SetRagdoll(bool willRagdoll)
    {
        IsRagdoll = willRagdoll;
        
        // Turn off the animator or else it will not ragdoll
        m_animationController.Animator.enabled = !willRagdoll;
        m_ragdollController.RagdollPlayer(willRagdoll);
    }

}
