using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class ObjectProductionService : Service, IObjectProductionService
{
    private readonly IObjectsConfig _objectsConfig;
    private readonly Contexts _contexts;

    public ObjectProductionService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
        _objectsConfig = contexts.config.objectsConfig.value;
    }
    
    public List<(ObjectsConfigData.ObjectData, int, int)> GenerateObjects(List<ObjectsConfigData.ObjectData> availableObjects)
    {
        // int maxObjectLevel = _contexts.game.maxObjectLevel.Value;
        int availableObjectsCount = availableObjects.Count;
        int baseObjectCount = (int)(_contexts.game.maxProducedObjectCount.Value / (availableObjectsCount * 1.0f));
        int maxObjectLevelAllowed = _contexts.game.maxProducedObjectLevel.Value;

        List<(ObjectsConfigData.ObjectData, int, int)> generatedObjects = new List<(ObjectsConfigData.ObjectData, int, int)>();

        for (int i = 0; i < availableObjectsCount; i++)
        {
            var objectData = availableObjects[i];
            Random random = new Random(_contexts.game.currentLevelIndex.Value * (i + 1));
            int initialObjectCount = CalculateInitialObjectCount(maxObjectLevelAllowed, baseObjectCount, random);

            // Add the initial object count
            AddObjectToList(generatedObjects, objectData, 0, initialObjectCount);

            int remainingObjectCount = baseObjectCount - initialObjectCount;
            GenerateObjectsForHigherLevels(generatedObjects, objectData, maxObjectLevelAllowed, remainingObjectCount, random);
        }
        
        int totalCreatedObjects = generatedObjects.Sum(item => item.Item3);
        _contexts.game.ReplaceMaxProducedObjectCount(totalCreatedObjects);
        _contexts.game.ReplaceDebugLog("Total: x" + totalCreatedObjects);
        return generatedObjects;
    }

    private int CalculateInitialObjectCount(int maxLevel, int baseCount, Random random)
    {
        var initialObjectCount = 0;
        if (maxLevel == 0)
        {
            initialObjectCount = (baseCount / 2) % 2 == 0 ? baseCount : baseCount + 2;
        }
        else
        {
            initialObjectCount = (int)(baseCount / (random.NextDouble() * (3 - 1.3) + 1.3));
        }
        
        return initialObjectCount % 2 != 0 ? initialObjectCount + 1 : initialObjectCount;
    }

    private void AddObjectToList(List<(ObjectsConfigData.ObjectData, int, int)> objectList, ObjectsConfigData.ObjectData objectData, int level, int count)
    {
        objectList.Add((objectData, level, count));
        Debug.Log($"{objectData.type}-{level}: x{count}");
    }

    private void GenerateObjectsForHigherLevels(List<(ObjectsConfigData.ObjectData, int, int)> objectList, ObjectsConfigData.ObjectData objectData, int maxLevel, int remainingCount, Random random)
    {
        var previousLevelCounts = new List<int>();
        var previousCounts = new List<int>();

        int objectsCount = remainingCount;
        

        for (int level = 1; level <= maxLevel; level++)
        {
            previousLevelCounts.Add(objectList.FirstOrDefault(item => item.Item1 == objectData && item.Item2 == level - 1).Item3);
            
            var restPreviousLevelCount = CalculateRestPreviousLevelCount(previousLevelCounts);
            previousCounts.Add(restPreviousLevelCount);
            Debug.Log(string.Join(", ", previousLevelCounts)); // 10, 3, 3
            Debug.Log("Rest: " + string.Join(", ", restPreviousLevelCount));
            
            int currentLevelCount = CalculateLevelObjectCount(level, maxLevel, objectsCount, restPreviousLevelCount, random);
            AddObjectToList(objectList, objectData, level, currentLevelCount);
            objectsCount -= currentLevelCount;
        }
    }
    
    private int CalculateRestPreviousLevelCount(List<int> previousLevelCounts)
    {
        int length = previousLevelCounts.Count;

        if (length == 1)
        {
            return previousLevelCounts[0] / 2;
        }
        else
        {
            List<int> sublist = previousLevelCounts.GetRange(0, length - 1);
            int sumOfSublist = CalculateRestPreviousLevelCount(sublist);
            return (sumOfSublist + previousLevelCounts[length - 1]) / 2;
        }
    }

    private int CalculateLevelObjectCount(int currentLevel, int maxLevel, int remainingCount, int previousCounts, Random random)
    {
        int count = currentLevel == maxLevel ? remainingCount : (int)(remainingCount / (random.NextDouble() * (3 - 1.8) + 1.8));
        
        count = AdjustCountBasedOnPreviousSum(previousCounts, count);

        return count;
    }
    
    private int AdjustCountBasedOnPreviousSum(int previousCounts, int currentCount)
    {
        if (previousCounts % 2 == 0)
        {
            return currentCount % 2 == 0 ? currentCount : currentCount + 1;
        }
        else
        {
            return currentCount % 2 == 0 ? currentCount + 1 : currentCount;
        }
    }
}