
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] lootDrops;
    [SerializeField] private HealthHandler playerHealthHandler;
    [SerializeField] private Transform[] spawnPositions;
    private ObjectPool<GameObject> propPool;
    [SerializeField] private bool collectionCheck = true;
    private int defaultCapacity;
    private int maxCount;
    [SerializeField] private float timeBetweenSpawns = 5f;
    [SerializeField] private float timeBetweenSpawnTrials = 5f;
    [SerializeField] private LayerMask dropLayer;
    private float nextSpawn;
    private bool isGameOver = false;
    private Coroutine spawnCoroutine;

    private List<Vector3> availablePos = new List<Vector3>();
    void Start()
    {
        defaultCapacity = spawnPositions.Length;
        maxCount = defaultCapacity;
        InitializeAvailablePosList();
        propPool = new ObjectPool<GameObject>(OnPropCreation, OnGettingProp, OnReleasingProp, OnDestroyingProp, collectionCheck, defaultCapacity, maxCount);
    }
    private void InitializeAvailablePosList()
    {
        foreach(Transform pos in spawnPositions)
        {
            availablePos.Add(pos.position);
        }
    }
    private void OnEnable()
    {
        if(playerHealthHandler != null)
        {
            playerHealthHandler.OnDeath += SetIsGameOverTrue;
        }
    }
    private void OnDisable()
    {
        if(playerHealthHandler != null)
        {
            playerHealthHandler.OnDeath -= SetIsGameOverTrue;
        }
    }
    void Update()
    {
        SpawnRepeater();
    }
    public GameObject OnPropCreation()
    {
        int randIndex = Random.Range(0, lootDrops.Length);
        GameObject prop = Instantiate(lootDrops[randIndex]);
        ICollectableLoot collectable = prop.GetComponent<ICollectableLoot>();
        if (collectable != null) { 
            collectable.AssignPool(propPool);
            Debug.Log("successfully created the box");
        }
        return prop;
    }
    public void OnGettingProp(GameObject prop)
    {
        prop.SetActive(true);
    }
    public void OnReleasingProp(GameObject prop)
    {
        availablePos.Add(prop.transform.position);
        prop.SetActive(false);
    }
    public void OnDestroyingProp(GameObject prop)
    {
        Destroy(prop);
    }
    
    private void SpawnRepeater()
    {
        if (!isGameOver)
        {
            if(spawnCoroutine == null)
                spawnCoroutine = StartCoroutine(HandleNextSpawnPos());

        }
    }
    private IEnumerator HandleNextSpawnPos()
    {
        int trialsCount = 3;
        
        for(int i=0; i < trialsCount; i++)
        {
            Vector3 nextPos = GetAvailablePos();
            if(nextPos.x >= Mathf.Infinity)
            {
                Debug.Log("Failed to find unoccupied pos, retrying");
                yield return new WaitForSeconds(timeBetweenSpawnTrials);
                continue;
            }
            if (propPool.CountActive < defaultCapacity)
            {
                GameObject drop = propPool.Get();
                drop.transform.position = nextPos;
                availablePos.Remove(nextPos);
                Debug.Log("Spawned loot at: " + nextPos);

            }
            break;
        }
        yield return new WaitForSeconds(timeBetweenSpawns);
        spawnCoroutine = null;
        yield return null;
    }
    
    private Vector3 GetAvailablePos()
    {
        if (spawnPositions.Length <= 0) return new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        
        if (availablePos.Count >0)
        {
            Vector3 randPos = availablePos[Random.Range(0, availablePos.Count)];
            Collider[] col = Physics.OverlapSphere(randPos, 1, dropLayer);
           
            if (col.Length <=0)
            {
                return randPos;
            }
        }
        return new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
    }

    public void SetIsGameOverTrue()
    {
        isGameOver = true;
    }
}

//Deprecated
/*
private void StartPropSpawning()
{
    if (!isGameOver && Time.time > nextSpawn)
    {
        nextSpawn = Time.time + timeBetweenSpawns / 2.5f;
        StartCoroutine(independantSpawnRoutine());
    }
}

 private IEnumerator independantSpawnRoutine()
    {
        
            if(propPool.CountActive<defaultCapacity && spawnPositions.Length > 0)
            {
                Vector3 dropPos = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
                bool isOccupied = true;
                int counter = 10;
                while (isOccupied && counter>0)
                {
                    Collider[] col = Physics.OverlapSphere(dropPos, 1, dropLayer);
                    if (col.Length < 1)
                    {
                        GameObject drop = propPool.Get();
                        drop.transform.position = dropPos;
                        isOccupied = false;
                        yield return null;
                    }
                    dropPos = spawnPositions[Random.Range(0, spawnPositions.Length)].position;
                    counter--;
                }
                
            }
       }
 
*/
