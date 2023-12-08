
using System;
using System.Collections.Generic;
using Udo.PoolManager;
using UnityEngine;

public class ObjectPoolInitializer : MonoBehaviour
{
    [SerializeField] private List<ObjectPool> _pools;
    private IObjectPoolService _objectPoolService;
    private Contexts _contexts;

    private void Start()
    {
        _contexts = Contexts.sharedInstance;
        _objectPoolService = new ObjectPoolService(_contexts);
        _objectPoolService.InitializePools(_pools);
    }
}