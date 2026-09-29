using UnityEngine;
using UnityEngine.Pool;

public class HealthBoxLogic : MonoBehaviour, ICollectableLoot
{
    [SerializeField] private float HealingAmount = 50f;
    private ObjectPool<GameObject> lootDropPool;
    private Collider playerCol;
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerCol = col;
            HandleCollectableLogic();
        }
    }
    public void SetHealingAmount(float health)
    {
        this.HealingAmount = health;
    }

    public void AssignPool(ObjectPool<GameObject> objPool)
    {
        this.lootDropPool = objPool;
    }

    public void HandleCollectableLogic()
    {
        HealthHandler playerHealth = playerCol.gameObject.GetComponent<HealthHandler>();
        if (playerHealth != null) playerHealth.AddHealth(HealingAmount);
        if (lootDropPool != null) lootDropPool.Release(this.gameObject);
        else gameObject.SetActive(false);
    }
}
