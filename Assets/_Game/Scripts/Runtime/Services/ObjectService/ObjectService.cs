using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    
    public Dictionary<int, ObjectsConfigData.ObjectData> CalculateObjectsWillProduced(List<ObjectsConfigData.ObjectData> availableObjects)
    {
        var objectStack = new Dictionary<int, ObjectsConfigData.ObjectData>(); // object level, object data
        var maxObjectLevel = _contexts.game.maxObjectLevel.Value;
        var maxProducedObjectCount = _contexts.game.maxProducedObjectCount.Value;
        var maxProducedObjectLevel = _contexts.game.maxProducedObjectLevel.Value;

        // Group objects by type
        var groupedObjects = availableObjects.GroupBy(obj => obj.type);

        foreach (var group in groupedObjects)
        {
            int objectCount = group.Count();
            int level = 1;
            while (objectCount > 0 && objectStack.Count < maxProducedObjectCount)
            {
                if (level > maxProducedObjectLevel)
                {
                    break; // Skip if the level exceeds the maximum allowed level
                }

                int objectsAtCurrentLevel = (int)Math.Pow(2, level - 1);
                int producibleObjectsCount = Math.Min(objectCount / objectsAtCurrentLevel, maxProducedObjectCount - objectStack.Count);

                if (producibleObjectsCount > 0)
                {
                    objectStack[level] = group.First(); // Assuming each level has a unique object representation
                    objectCount -= producibleObjectsCount * objectsAtCurrentLevel;
                }

                level++;
            }
        }

        return objectStack;
    }

    
    public void SetAvailableObjectByType(string objectType, bool isAvailable)
    {
        var objectIndex = _objectsConfig.Config.objects.FindIndex(o => o.type == objectType);

        if (objectIndex == -1)
            throw new KeyNotFoundException($"Object type {objectType} not found in config.");

        int availableObjectsBitmask = _contexts.game.availableObjects.Value;
        availableObjectsBitmask = isAvailable
            ? availableObjectsBitmask | (1 << objectIndex) // Set the bit at the object's index to 1
            : availableObjectsBitmask & ~(1 << objectIndex); // Set the bit at the object's index to 0

        _contexts.game.ReplaceAvailableObjects(availableObjectsBitmask);
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