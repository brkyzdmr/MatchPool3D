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

    public List<(ObjectsConfigData.ObjectData, int, int)> GenerateObjects(List<ObjectsConfigData.ObjectData> availableObjects)
    {
        var maxObjectLevel = _contexts.game.maxObjectLevel.Value;
        var availableObjectsCount = availableObjects.Count;
        var maxProducedObjectCount = _contexts.game.maxProducedObjectCount.Value / availableObjectsCount;
        var maxProducedObjectLevel = _contexts.game.maxProducedObjectLevel.Value;

        List<(ObjectsConfigData.ObjectData, int, int)> generatedObjects = new List<(ObjectsConfigData.ObjectData, int, int)>();
        // objectdata, level, count

        for (int i = 0; i < availableObjectsCount; i++)
        {
            Random random = new Random(_contexts.game.currentLevelIndex.Value * i);
            var previousLevelCounts = new List<int>();
            var lowestLevelCount = 0;
            
            if (maxProducedObjectLevel == 1)
            {
                if ((maxProducedObjectLevel / 2) % 2 == 0)
                {
                    lowestLevelCount = maxProducedObjectCount;
                }
                else
                {
                    lowestLevelCount = maxProducedObjectCount + 2;
                }
            }
            else
            {
                lowestLevelCount = (int)(maxProducedObjectCount / (random.NextDouble() * (3 - 1.3) + 1.3));
            }

            if (lowestLevelCount % 2 != 0)
            {
                lowestLevelCount += 1;
            }
            
            generatedObjects.Add((availableObjects[i], 0, lowestLevelCount));

            Debug.Log(generatedObjects[i].Item1.type + "-" + 0 + ": x" + lowestLevelCount);

            var objectsCount = maxProducedObjectCount - lowestLevelCount;
            var previousObjectsCount = new List<int>();
            

            for (int level = 1; level < maxProducedObjectLevel; level++)
            {
                var previosLevelCount1 = generatedObjects.Count(item => item.Item1 == availableObjects[i] && item.Item2 == level - 1);
                previousLevelCounts.Add(previosLevelCount1);
                var previousLevelCount2 = 0;

                // var currentObjectCount = generatedObjects.Count(item => item.Item1 == availableObjects[i]);

                for (int j = 0; j < previosLevelCount1; j++)
                {
                    previousLevelCount2 += (int)(previousLevelCounts[i] / (int)(Math.Pow(2, (previosLevelCount1 - i))));
                }
                
                previousObjectsCount.Add(previousLevelCount2);

                var currentLevelCount = 0;
                if (level == maxProducedObjectLevel)
                {
                    currentLevelCount = objectsCount;
                }
                else
                {
                    currentLevelCount = (int)(objectsCount / (random.NextDouble() * (3 - 1.8) + 1.8));
                }

                if (previousObjectsCount.Sum() % 2 == 0)
                {
                    if (currentLevelCount % 2 != 0)
                    {
                        currentLevelCount += 1;
                    }
                }
                else
                {
                    if (currentLevelCount % 2 == 0)
                    {
                        currentLevelCount += 1;
                    }
                }

                var currentObjects = (availableObjects[i], level, currentLevelCount);
                Debug.Log(currentObjects.Item1.type + "-" + level + ": x" + currentLevelCount);
                
                generatedObjects.Add((availableObjects[i], level, currentLevelCount));
                objectsCount -= currentLevelCount;
            }
        }

        return generatedObjects;
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