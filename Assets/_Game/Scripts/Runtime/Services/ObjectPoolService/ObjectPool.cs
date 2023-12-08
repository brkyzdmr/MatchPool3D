using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class ObjectPool
{
    public int MaximumInstances => maximumInstances;
    public Pools.Types PoolType
    {
        get => poolType;
        set => poolType = value;
    }
    
    public GameObject prefab;
    public int maximumInstances;
    public Pools.Types poolType;

    private Queue<GameObject> _inactiveObjects;
    private GameObject _poolContainer;

    public void InitializePool()
    {
        _inactiveObjects = new Queue<GameObject>();
        _poolContainer = new GameObject($"[Pool - {poolType}]");
        Object.DontDestroyOnLoad(_poolContainer);

        for (int i = 0; i < maximumInstances; i++)
        {
            var instance = CreateNewInstance();
            DeactivateAndEnqueue(instance);
        }
    }

    private GameObject CreateNewInstance()
    {
        var instance = Object.Instantiate(prefab, _poolContainer.transform, true);
        return instance;
    }

    private void DeactivateAndEnqueue(GameObject instance)
    {
        instance.SetActive(false);
        _inactiveObjects.Enqueue(instance);
    }

    public GameObject GetNextObject()
    {
        if (_inactiveObjects.Count == 0)
        {
            Debug.LogWarning($"[ObjectPool] {poolType} - Ran out of instances. Instantiating new one.");
            var newInstance = CreateNewInstance();
            DeactivateAndEnqueue(newInstance);
        }

        var nextObject = _inactiveObjects.Dequeue();
        nextObject.SetActive(true);
        return nextObject;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.transform.SetParent(_poolContainer.transform);
        _inactiveObjects.Enqueue(obj);
    }
}