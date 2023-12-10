using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolService : Service, IObjectPoolService
{
    private Dictionary<Pools.Types, ObjectPool> _objectPools;

    public ObjectPoolService(Contexts contexts) : base(contexts)
    {
    }

    public void InitializePools(List<ObjectPool> pools)
    {
        _objectPools = new Dictionary<Pools.Types, ObjectPool>();

        foreach (var pool in pools)
        {
            pool.InitializePool();
            _objectPools[pool.poolType] = pool;
        }
    }

    public GameObject Spawn(Pools.Types poolType, Vector3? position = null, Quaternion? rotation = null, Transform parent = null)
    {
        if (_objectPools == null)
        {
            Debug.LogError("Object pools not initialized.");
            return null;
        }

        if (!_objectPools.TryGetValue(poolType, out var pool))
        {
            Debug.LogError($"Pool Type {poolType} not found. Available pools: {string.Join(", ", _objectPools.Keys)}");
            return null;
        }

        var obj = pool.GetNextObject();

        if (obj == null)
        {
            Debug.LogError($"Pool {poolType} is out of objects.");
            return null;
        }

        obj.transform.SetParent(parent);
        obj.transform.position = position ?? Vector3.zero;
        obj.transform.rotation = rotation ?? Quaternion.identity;
        obj.SetActive(true);

        return obj;
    }


    public void Despawn(Pools.Types poolType, GameObject obj)
    {
        if (!_objectPools.TryGetValue(poolType, out var pool))
        {
            Debug.LogError($"Pool Type {poolType} not found.");
            return;
        }

        obj.SetActive(false);
        pool.ReturnObjectToPool(obj);
    }
}