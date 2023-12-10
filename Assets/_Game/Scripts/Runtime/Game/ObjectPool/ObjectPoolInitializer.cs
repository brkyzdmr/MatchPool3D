using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolInitializer : MonoBehaviour
{
    [SerializeField] private List<ObjectPool> _pools;
    private IObjectPoolService _objectPoolService;
    private Contexts _contexts;

    private void Awake()
    {
        _contexts = Contexts.sharedInstance;
        _objectPoolService = Services.GetService<IObjectPoolService>();
        
        if (_pools == null || _pools.Count == 0)
        {
            Debug.LogError("ObjectPoolInitializer: No pools provided for initialization.");
            return;
        }

        _objectPoolService.InitializePools(_pools);
    }
}