using System;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    public event Action OnDeath;
    public event Action OnHit;
    private bool isAlive = true;
    public float Health { set => health = value; }
    public void DecrementHealth(float dmg)
    {
        if (!isAlive) return;

        health -= dmg;
        if (health <= 0)
        {
            isAlive = false;
            health = 0;
            OnDeath?.Invoke();
            return;
        }
        Debug.Log("Hit delievered");
        OnHit?.Invoke();
    }
    public void AddHealth(float healing)
    {
        if (!isAlive) return;
        health += healing;
    }
    public float getHealth()
    {
        return this.health;
    }

    public void ResetHealth(float health)
    {
        isAlive = true;
        this.health = health;
    }
   
}
