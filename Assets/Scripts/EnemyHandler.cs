using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.Pool;
using System.Collections;
public class EnemyHandler : MonoBehaviour
{
    [SerializeField] private HealthHandler health;
    [SerializeField] private Collider enemyCollider;
    private EnemyVisual enemyVisual;
    private EnemyMovement enemyMovement;
    private EnemyAttack enemyAttack;
    private ObjectPool<GameObject> enemySpawner;
    private Coroutine hitCoroutine;
    private NavMeshAgent aiBrain;
    private Rigidbody rb;
    public ObjectPool<GameObject> ObjectPool { set => enemySpawner = value; get => enemySpawner; }
    void Awake()
    {
        aiBrain = GetComponent<NavMeshAgent>();
        enemyVisual = GetComponent<EnemyVisual>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody>();

        
        //HandleEnemyChase();
    }
    private void HandleEnemyChase()
    {
        aiBrain.isStopped = false;
        if(enemyVisual != null)
        {
            enemyVisual.PlayWalking();
        }
        if(enemyMovement != null)
        {
            enemyMovement.enabled = true;
            enemyMovement.SetCanMove(true);
        }
        if (enemyAttack != null)
        {
            enemyAttack.enabled = true;
            enemyAttack.SetCanAtk(true);
        }
    }
    public void RegenerateEnemy(Vector3 spawnPos)
    {
        aiBrain.enabled = true;
        aiBrain.Warp(spawnPos);
        aiBrain.ResetPath();
        aiBrain.isStopped = false;

        if (enemyCollider != null) enemyCollider.enabled = true;
        if (health != null) health.ResetHealth();
        
        
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
        aiBrain.isStopped = true;
        aiBrain.ResetPath();
        aiBrain.enabled = false;
        
        if (enemyVisual != null) { 
            enemyVisual.PlayDeath();
            
        }
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
        if (enemyMovement != null)
        {
            enemyMovement.SetCanMove(false);
            enemyMovement.enabled = false;
        }
        aiBrain.isStopped= true;


    }
    public void StunEnemy()
    {
        if (enemyAttack != null) enemyAttack.enabled = false;
        StopEnemy();
    }
    public void UnStunEnemy()
    {
        if (enemyAttack != null) enemyAttack.enabled = true;
        HandleEnemyChase();
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
        if (enemyVisual != null) enemyVisual.PlayHit();
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
