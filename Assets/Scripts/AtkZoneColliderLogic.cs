using System;
using UnityEngine;

public class AtkZoneColliderLogic : MonoBehaviour
{
    public event Action<AttacksEnum> OnRangeEnter;
    [SerializeField] private AttacksEnum atk;
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            OnRangeEnter?.Invoke(atk);
        }
    }
    
}
