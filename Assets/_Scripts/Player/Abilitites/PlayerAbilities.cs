using System.Collections;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] private PlayerMovement m_playerMovement;
    
    [Header("Charge")]
    [SerializeField] private float m_chargeSpeedMultiplier;
    [SerializeField] private float m_chargeDuration;
    [SerializeField] private float m_chargeSlowDownTime;

    public bool IsCharging;

    // Called as event from Input
    public void DoChargeAbility()
    {
        if (IsCharging) return;
        
        StartCoroutine(Charge());
    }
    
    private IEnumerator Charge()
    {
        IsCharging = true;
        m_playerMovement.MovementSpeedMultiplier = m_chargeSpeedMultiplier;
        // Duration of charge
        for (float timeSpent = 0; timeSpent < m_chargeDuration; timeSpent += Time.deltaTime)
        {
            yield return null;
        }
        
        // Duration to slowdown back to normal speed assumed is 1
        float differenceInSpeedValue = m_chargeSpeedMultiplier - 1f;
        for (float timeSpent = 0f; timeSpent < m_chargeSlowDownTime; timeSpent += Time.deltaTime)
        {
            float normalizedTime = timeSpent / m_chargeSlowDownTime;
            float nextSpeedMultiplier = m_chargeSpeedMultiplier - differenceInSpeedValue * normalizedTime;
            
            if (nextSpeedMultiplier < 1)
                yield break;
            
            m_playerMovement.MovementSpeedMultiplier = nextSpeedMultiplier;
            yield return null;
        }

        m_playerMovement.MovementSpeedMultiplier = 1f;
        IsCharging = false;
    }
}
