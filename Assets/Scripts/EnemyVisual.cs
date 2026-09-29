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
    [SerializeField] private ParticleSystem bloodSplatterVFX;
    [SerializeField] private ParticleSystem fallOnGroundVFX;
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

    public void PlayBloodVFX(Vector3 position, Quaternion rotation)
    {
        if (bloodSplatterVFX != null)
        {
            bloodSplatterVFX.transform.position = position;
            bloodSplatterVFX.transform.rotation = rotation;
            bloodSplatterVFX.Play();
        }
    }
    public void PlayFallOnGroundVFX()
    {
        if(fallOnGroundVFX != null)
        {
            //fallOnGroundVFX.transform.position = new Vector3(transform.position.x,0f,transform.position.z);
            fallOnGroundVFX.Play();
        }
    }
}
