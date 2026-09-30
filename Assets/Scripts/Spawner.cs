using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnLocations;
    private int waveSpawnCount = 5;
    private int originalWaveSpawnCount = 5;
    private int remainingSpawnCount;
    private int currEnemyCount = 0;
    public event Action OnWaveCompleted;

    [SerializeField] private HealthHandler player;
    private ObjectPool<GameObject> enemyPool;
    [SerializeField] private int defaultSpawn = 8;
    [SerializeField] private int maxSpawnOnScene =15;
    [SerializeField] private bool collectionCheck = true;
    private bool isSpawning = false;
    private SpawnerUIHandler spawnerUI;
    private EnemyHandler[] enemies;
    void Awake()
    {
       enemyPool = new ObjectPool<GameObject>(CreateEnemy,OnGetFromPool,OnReleaseToPool,OnDestroyPooledObject,
           collectionCheck, defaultSpawn, maxSpawnOnScene);
    }

    private GameObject CreateEnemy()
    {
        GameObject enemyToSpawn = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
        GameObject enemy = Instantiate(enemyToSpawn);
        EnemyHandler enemyHandler = enemy.GetComponent<EnemyHandler>();
        spawnerUI = GetComponent<SpawnerUIHandler>();
        if (enemyHandler != null) enemyHandler.ObjectPool = enemyPool;
        return enemy;
        
    }
    private void OnGetFromPool(GameObject enemy)
    {
        enemy.gameObject.SetActive(true);
        HealthHandler enemyHealth = enemy.GetComponent<HealthHandler>();
        if(enemyHealth != null) enemyHealth.OnDeath += UpdateUI;
    }
    private void OnReleaseToPool(GameObject enemy)
    {
        HealthHandler enemyHealth = enemy.GetComponent<HealthHandler>();
        if(enemyHealth !=null)enemyHealth.OnDeath -= UpdateUI;
        enemy.gameObject.SetActive(false);
    }
    private void OnDestroyPooledObject(GameObject enemy)
    {
        Destroy(enemy);
    }
    void Update()
    {
        CheckWaveCompletion();
    }
    public void SpawnEnemies()
    {
        if (enemyPrefabs.Length <= 0) return;
        if (spawnLocations.Length <= 0) return;
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(EnemySpawningNumerator());
        }
        
    }
    public void StartSpawning()
    {
        SpawnEnemies();
    }
    public void SetSpawnCount(int count)
    {
        this.originalWaveSpawnCount = count;
        this.remainingSpawnCount = count;
        this.waveSpawnCount = count;
        if(spawnerUI !=null) this.spawnerUI.SetEnemyCountText(remainingSpawnCount, originalWaveSpawnCount);
    }
    private void UpdateUI()
    {
        remainingSpawnCount--;
        this.spawnerUI.SetEnemyCountText(remainingSpawnCount, originalWaveSpawnCount);
    }
    private IEnumerator EnemySpawningNumerator()
    {
        while (waveSpawnCount > 0)
        {
            currEnemyCount = FindObjectsByType<EnemyMovement>().Length;
            if (enemyPool.CountActive < maxSpawnOnScene)
            {
                
                GameObject enemy = enemyPool.Get();
                Vector3 spawnPos = spawnLocations[UnityEngine.Random.Range(0, spawnLocations.Length)].position;
                  
                EnemyHandler enemyHandler = enemy.GetComponent<EnemyHandler>();
                if (enemyHandler != null) enemyHandler.RegenerateEnemy(spawnPos);
                
                waveSpawnCount--;
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
            
        }
        
        yield return new WaitForSeconds(2);
        
        isSpawning = false;
    }
    private void CheckWaveCompletion()
    {
        if(!isSpawning && waveSpawnCount <=0 && enemyPool.CountActive == 0)
        {
            waveSpawnCount = -1;
            if(player !=null && player.IsAliveProperty)OnWaveCompleted?.Invoke();
        }
    }
    private void OnEnable()
    {
        player.OnDeath += HandleSpawnerReset;
    }
    private void OnDisable()
    {
        player.OnDeath -= HandleSpawnerReset;
    }
    private void HandleSpawnerReset()
    {
        this.waveSpawnCount = 0;
        this.currEnemyCount = 0;
        enemies = FindObjectsByType<EnemyHandler>();
        foreach(EnemyHandler enemy in enemies)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                enemy.ObjectPool.Release(enemy.gameObject);
            }
        }
    }
   
}
