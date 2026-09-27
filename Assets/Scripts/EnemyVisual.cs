using System;
using System.Globalization;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private const string IsWalking = "IsWalking";
    [SerializeField] private const string AttackTrigger = "Attack";
    [SerializeField] private const string DeathTrigger = "Death";
    [SerializeField] private const string HitTrigger = "Hit";
    [SerializeField] private const string RegenerateTrigger = "OnRegenerate";

    [SerializeField] private Animator animator;
    
    public void PlayWalking()
    {
        animator.SetBool(IsWalking, true);
    }
    public void StopWalking()
    {
        animator.SetBool(IsWalking, false);
    }
    public void PlayAttack(AttacksEnum atkEnum)
    {
        switch (atkEnum)
        {
            case AttacksEnum.HandAtk: animator.SetTrigger(AttackTrigger); break;
            case AttacksEnum.VomitAtk: Debug.Log("Play idle"); break;
            default: Debug.LogError("Attack animation not supported"); break;
        }
        
    }
    public void PlayDeath()
    {
        animator.SetTrigger(DeathTrigger);
    }
    public void PlayHit()
    {
        animator.SetTrigger(HitTrigger);
    }
    public void ExitDeathAnimation()
    {
        animator.SetTrigger(RegenerateTrigger);
    }

    
}
