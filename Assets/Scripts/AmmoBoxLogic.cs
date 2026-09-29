using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
public class AmmoBoxLogic : MonoBehaviour, ICollectableLoot
{
    
    private ObjectPool<GameObject> objectPool;
    public ObjectPool<GameObject> ObjectPool { set => objectPool = value; }

    Dictionary<AmmoEnum, int> ammoBoxContent;
    Collider playerCollider;
    public void AssignPool(ObjectPool<GameObject> pool)
    {
        this.objectPool = pool;
    }
    public void TransferContent(Dictionary<AmmoEnum, int> ammoBoxContent, Collider playerCollider)
    {
        this.ammoBoxContent = ammoBoxContent;
        this.playerCollider = playerCollider;
        HandleCollectableLogic();
    }
    public void HandleCollectableLogic()
    {
        PlayerInventory playerInvenotry = playerCollider.gameObject.GetComponent<PlayerInventory>();
        if (playerInvenotry != null) playerInvenotry.AddItemsToInventory(ammoBoxContent);
        if (objectPool != null)
        {
            Debug.Log("Released box to pool");
            objectPool.Release(this.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
            Debug.LogError("ObjectPool on box is null");
        }
       
    }
}
