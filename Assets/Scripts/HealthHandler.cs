using System;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currHealth;
    public event Action OnDeath;
    public event Action OnHit;
    public event Action OnHealed;
    private bool isAlive = true;
    public float Health { set => currHealth = value; }
    public void Start()
    {
        currHealth = maxHealth;
    }
    public void DecrementHealth(float dmg)
    {
        if (!isAlive) return;

        currHealth -= dmg;
        if (currHealth <= 0)
        {
            isAlive = false;
            currHealth = 0;
            OnDeath?.Invoke();
            return;
        }
        Debug.Log("Hit delievered");
        OnHit?.Invoke();
    }
    public void AddHealth(float healing)
    {
        if (!isAlive) return;
        currHealth = Mathf.Min(currHealth + healing, maxHealth);
        OnHealed?.Invoke();
    }
    public float getHealth()
    {
        return this.currHealth;
    }

    public void ResetHealth(float health)
    {
        isAlive = true;
        this.currHealth = health;
    }
   
}
