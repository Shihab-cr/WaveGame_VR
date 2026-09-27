
using UnityEngine;
using UnityEngine.Pool;
public interface ICollectableLoot
{
    void AssignPool(ObjectPool<GameObject> objPool);
    void HandleCollectableLogic();
}
