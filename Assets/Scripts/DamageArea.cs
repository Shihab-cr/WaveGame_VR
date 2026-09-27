using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private EnemyAttack enemyAtk;
    [SerializeField] private AnimationEventRelay eventRelay;
    private Collider collider;
    private float damage;
    void Awake()
    {
        damage = enemyAtk.getDmg();
    }
    void Start()
    {
        collider = GetComponent<Collider>();
        DisableDamageZone();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthHandler playerHealth = other.GetComponent<HealthHandler>();
            playerHealth.DecrementHealth(damage);
            DisableDamageZone();
        }
    }
    public void DisableDamageZone()
    {
        if (collider != null) collider.enabled = false;
    }
    public void EnableDamageZone()
    {
        if (collider != null) collider.enabled = true;
    }

    private void HandleRelay(string msg)
    {
        if (msg.Equals("Disable"))
        {
            DisableDamageZone();
        }
        else if (msg.Equals("Enable"))
        {
            EnableDamageZone();
        }
    }
    private void OnEnable()
    {
        eventRelay.OnAnimationEvent += HandleRelay; 
    }
    private void OnDisable()
    {
        eventRelay.OnAnimationEvent -= HandleRelay;
    }
}
