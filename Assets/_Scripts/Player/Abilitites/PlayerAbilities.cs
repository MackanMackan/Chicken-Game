using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] private PlayerController m_playerController;

    [Header("Charge")] [SerializeField] private float m_chargeSpeedMultiplier;
    [SerializeField] private float m_chargeDuration;
    [SerializeField] private float m_chargeSlowDownTime;

    [Header("Stuck")] [SerializeField] private Rigidbody m_headRigidbody;
    [SerializeField] private float m_unstuckCooldown = 5f;
    [SerializeField] private UnityEvent m_gotStuck;
    [SerializeField] private UnityEvent m_gotUnstuck;

    private PlayerMovement m_playerMovement;
    private Joint m_joint;
    private bool m_canGetStuck = true;

    public bool IsCharging;
    public bool IsStuck;

    private void Start()
    {
        m_playerMovement = m_playerController.m_playerMovement;
    }

    // Charge Ability
    // Called as event from Input
    public void DoChargeAbility()
    {
        if (IsCharging || IsStuck) return;

        StartCoroutine(Charge());
    }

    private IEnumerator Charge()
    {
        IsCharging = true;
        m_playerMovement.MovementSpeedMultiplier = m_chargeSpeedMultiplier;

        // Duration of charge
        for (float timeSpent = 0; timeSpent < m_chargeDuration; timeSpent += Time.deltaTime)
        {
            // If Stuck, set back to normal speeds
            if (IsStuck)
            {
                m_playerMovement.MovementSpeedMultiplier = 1f;
                IsCharging = false;
                yield break;
            }

            yield return null;
        }
        
        // TODO: Split this to its own method
        // Duration to slowdown back to normal speed assumed is 1
        float differenceInSpeedValue = m_chargeSpeedMultiplier - 1f;
        for (float timeSpent = 0f; timeSpent < m_chargeSlowDownTime; timeSpent += Time.deltaTime)
        {
            float normalizedTime = timeSpent / m_chargeSlowDownTime;
            float nextSpeedMultiplier = m_chargeSpeedMultiplier - differenceInSpeedValue * normalizedTime;

            // If Stuck, set back to normal speeds
            if (nextSpeedMultiplier < 1 || IsStuck)
            {
                m_playerMovement.MovementSpeedMultiplier = 1f;
                IsCharging = false;
                yield break;
            }

            m_playerMovement.MovementSpeedMultiplier = nextSpeedMultiplier;
            yield return null;
        }

        m_playerMovement.MovementSpeedMultiplier = 1f;
        IsCharging = false;
    }

    // Stuck Ability
    // Event from axe on head
    public void DoStuck(Collider other)
    {
        if (IsStuck || !m_canGetStuck) return;
        
        m_joint = m_headRigidbody.AddComponent<FixedJoint>();
        m_joint.connectedBody = other.attachedRigidbody;
        m_canGetStuck = false;
        IsStuck = true;
        m_gotStuck?.Invoke();
    }

    // Called from Jump Event Input
    public void DoUnstuck()
    {
        if (!IsStuck) return;

        Destroy(m_joint);
        StartCoroutine(UnstuckCooldown());
        IsStuck = false;
        m_gotUnstuck?.Invoke();
    }

    private IEnumerator UnstuckCooldown (){
        for (float timeSpent = 0; timeSpent < m_unstuckCooldown; timeSpent += Time.deltaTime)
        {
            yield return null;
        }
        m_canGetStuck = true;
    }

}
