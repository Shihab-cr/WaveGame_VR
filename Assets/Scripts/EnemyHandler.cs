using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.Pool;
using System.Collections;
public class EnemyHandler : MonoBehaviour
{
    [SerializeField] private HealthHandler health;
    [SerializeField] private Collider enemyCollider;
    [SerializeField] private float enemyHealth;
    private EnemyVisual enemyVisual;
    private EnemyMovement enemyMovement;
    private EnemyAttack enemyAttack;
    private ObjectPool<GameObject> enemySpawner;
    private Coroutine hitCoroutine;
    public ObjectPool<GameObject> ObjectPool { set => enemySpawner = value; }
    void Start()
    {
        
        enemyVisual = GetComponent<EnemyVisual>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
        if (health != null) health.Health = enemyHealth;
        HandleEnemyChase();
    }
    void Update()
    {
        if(enemyMovement != null)
        {
            enemyMovement.MoveTowardsTarget();
        }
    }
    private void HandleEnemyChase()
    {
        if(enemyVisual != null)
        {
            enemyVisual.PlayWalking();
        }
        if(enemyMovement != null)
        {
            enemyMovement.SetCanMove(true);
        }
        if (enemyAttack != null)
        {
            enemyAttack.SetCanAtk(true);
        }
    }
    public void RegenerateEnemy()
    {
        NavMeshAgent aiBrain = GetComponent<NavMeshAgent>();
        Rigidbody rb = GetComponent<Rigidbody>();
        aiBrain.enabled = true;
        
        if (enemyCollider != null) enemyCollider.enabled = true;
        if (health != null) health.ResetHealth(enemyHealth);
        
        
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.freezeRotation = true;
        }

        HandleEnemyChase();
        
    }
    
        
        
    
    private void HandleEnemyDeath()
    {
        StopEnemy();
        NavMeshAgent aiBrain = GetComponent<NavMeshAgent>();
        Rigidbody rb = GetComponent<Rigidbody>();
        aiBrain.enabled = false;
        
        if(enemyVisual != null) enemyVisual.PlayDeath();
        if(enemyAttack != null) enemyAttack.SetCanAtk(false);
        if (enemyCollider != null) enemyCollider.enabled = false;
        
        if(rb != null)
        {
            rb.isKinematic = true;
            rb.freezeRotation = true;
        }
        if (hitCoroutine != null) StopCoroutine(hitCoroutine);
        StartCoroutine(EnemyReleaseToPoolCoroutine());
        
    }
    private IEnumerator EnemyReleaseToPoolCoroutine()
    {
        yield return new WaitForSeconds(2);
        if(enemySpawner != null)
        {
            enemySpawner.Release(this.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void StopEnemy()
    {
        if(enemyVisual != null)
        {
            enemyVisual.StopWalking();
        }
        if(enemyMovement != null)
        {
            enemyMovement.SetCanMove(false);
        }
        
    }
    
    public void RecvAtkType(AttacksEnum atkEnum)
    {
        if (atkEnum.Equals(AttacksEnum.VomitAtk))
        {
            // if (enemyVisual != null) enemyVisual.StopWalking();
            // if (enemyMovement != null) enemyMovement.SetCanMove(false);
            Debug.Log("Enemy vomitted at player");
        }
        if (enemyVisual != null) enemyVisual.PlayAttack(atkEnum);
    }
    
    private void HandleEnemyHit()
    {
        if(hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }
        hitCoroutine = StartCoroutine(OnEnemyHit());
    }
    private IEnumerator OnEnemyHit()
    {
        StopEnemy();
        enemyVisual.PlayHit();
        enemyAttack.SetCanAtk(false);
        yield return new WaitForSeconds(0.35f);
        HandleEnemyChase();
    }
    private void OnEnable()
    {
        if (health == null) return;
        health.OnDeath += HandleEnemyDeath;
        health.OnHit += HandleEnemyHit;
        if (enemyVisual != null) enemyVisual.ExitDeathAnimation();
    }
    private void OnDisable()
    {
        if (health == null) return;
        health.OnDeath -= HandleEnemyDeath;
        health.OnHit -= HandleEnemyHit;
    }

    
}
