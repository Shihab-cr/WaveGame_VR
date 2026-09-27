using UnityEngine;
using System;
public class EnemyAttack : MonoBehaviour
{
  
    [SerializeField] private float damage = 20f;
    [SerializeField] private AtkZoneColliderLogic handAtkRange;
    [SerializeField] private AtkZoneColliderLogic vomitAtkRange;
    private EnemyHandler enemyHandler;
    
    private bool canAtk = true;
    
    void Start()
    {
        enemyHandler = GetComponent<EnemyHandler>();
    }
    public void SetCanAtk(bool cAtk)
    {
        this.canAtk = cAtk;
    }

    private void HandleAtk(AttacksEnum atkEnum)
    {
        if (!canAtk) return;
        enemyHandler.RecvAtkType(atkEnum);
    }
    private void OnEnable()
    {
        if(handAtkRange != null)
        {
            handAtkRange.OnRangeEnter += HandleAtk;
        }
        if(vomitAtkRange != null)
        {
            vomitAtkRange.OnRangeEnter += HandleAtk;
        }
    }
    private void OnDisable()
    {
        if(handAtkRange != null)
        {
            handAtkRange.OnRangeEnter -= HandleAtk;
        }
        if(vomitAtkRange != null)
        {
            vomitAtkRange.OnRangeEnter -= HandleAtk;
        }
    }
    public float getDmg() { return damage; }
}
