using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class ObjectService : Service, IObjectService
{
    private readonly IObjectsConfig _objectsConfig;
    private readonly Dictionary<string, ObjectsConfigData.ObjectData> _objectDatas;
    private readonly Contexts _contexts;


    public ObjectService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
        _objectsConfig = contexts.config.objectsConfig.value;
        _objectDatas = _objectsConfig.Config.objects.ToDictionary(od => od.type, od => od);
    }

    public List<ObjectsConfigData.ObjectData> GetAllAvailableObjects()
    {
        var availableObjectsBitmask = _contexts.game.availableObjects.Value;
        return _objectDatas.Values
            .Where((o, i) => (availableObjectsBitmask & (1 << i)) != 0)
            .ToList();
    }

    public ObjectsConfigData.ObjectData GetRandomAvailableObject()
    {
        int availableObjectsBitmask = _contexts.game.availableObjects.Value;
        var availableObjects = _objectDatas.Values
            .Where((o, i) => (availableObjectsBitmask & (1 << i)) != 0)
            .ToList();
        
        return !availableObjects.Any() ? null : availableObjects[UnityEngine.Random.Range(0, availableObjects.Count)];
    }

    public void SetAvailableObjectByType(string objectType, bool isAvailable)
    {
        var objectIndex = FindObjectIndexByType(objectType);
        int availableObjectsBitmask = _contexts.game.availableObjects.Value;

        availableObjectsBitmask = UpdateBitmask(availableObjectsBitmask, objectIndex, isAvailable);
        _contexts.game.ReplaceAvailableObjects(availableObjectsBitmask);
    }

    public bool IsObjectInAvailableObjects(string objectType)
    {
        var objectIndex = FindObjectIndexByType(objectType);
        int availableObjectsBitmask = _contexts.game.availableObjects.Value;

        return IsBitSet(availableObjectsBitmask, objectIndex);
    }

    private int FindObjectIndexByType(string objectType)
    {
        var objectIndex = _objectsConfig.Config.objects.FindIndex(o => o.type == objectType);

        if (objectIndex == -1)
            throw new KeyNotFoundException($"Object type {objectType} not found in config.");

        return objectIndex;
    }

    private int UpdateBitmask(int bitmask, int index, bool setBit)
    {
        return setBit ? bitmask | (1 << index) : bitmask & ~(1 << index);
    }

    private bool IsBitSet(int bitmask, int index)
    {
        return (bitmask & (1 << index)) != 0;
    }

    public string GetObjectPath(ObjectsConfigData.ObjectData objectData, int level)
    {
        return objectData.levels[level];
    }

    public string GetObjectPath(string type, int level)
    {
        if (_objectDatas.TryGetValue(type, out var objectData))
            return objectData.levels[level];

        throw new KeyNotFoundException($"Object type {type} not found.");
    }
    
    public Sprite GetObjectSpriteByType(string type)
    {
        if (_objectDatas.TryGetValue(type, out var objectData))
        {
            return Resources.Load<Sprite>(objectData.shop.sprite);
        }
        else
        {
            throw new KeyNotFoundException($"Object type {type} not found.");
        }
    }
}