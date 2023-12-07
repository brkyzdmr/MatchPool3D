﻿using System.Collections.Generic;
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
            Random random = new Random(_contexts.game.currentLevelIndex.Value * i);
            int initialObjectCount = CalculateInitialObjectCount(maxObjectLevelAllowed, baseObjectCount, random);

            // Add the initial object count
            AddObjectToList(generatedObjects, objectData, 0, initialObjectCount);

            int remainingObjectCount = baseObjectCount - initialObjectCount;
            GenerateObjectsForHigherLevels(generatedObjects, objectData, maxObjectLevelAllowed, remainingObjectCount, random);
        }
        
        int totalCreatedObjects = generatedObjects.Sum(item => item.Item3);
        _contexts.game.ReplaceMaxProducedObjectCount(totalCreatedObjects);
        return generatedObjects;
    }

    private void AddObjectToList(List<(ObjectsConfigData.ObjectData, int, int)> objectList, ObjectsConfigData.ObjectData objectData, int level, int count)
    {
        objectList.Add((objectData, level, count));
        Debug.Log($"{objectData.type}-{level}: x{count}");
    }

    private int CalculateInitialObjectCount(int maxLevel, int baseCount, Random random)
    {
        var initialObjectCount = 0;
        if (maxLevel == 0)
        {
            initialObjectCount = baseCount;
        }
        else
        {
            initialObjectCount = (int)(baseCount / (random.NextDouble() * (3 - 1.3) + 1.3));
        }
        
        return initialObjectCount % 2 != 0 ? initialObjectCount + 1 : initialObjectCount;
    }

    private void GenerateObjectsForHigherLevels(List<(ObjectsConfigData.ObjectData, int, int)> objectList, ObjectsConfigData.ObjectData objectData, int maxLevel, int remainingCount, Random random)
    {
        var previousCounts = new List<int>();
        int objectsCount = remainingCount;

        for (int level = 1; level < maxLevel; level++)
        {
            int currentLevelCount = CalculateLevelObjectCount(level, maxLevel, objectsCount, previousCounts, random);
            AddObjectToList(objectList, objectData, level, currentLevelCount);
            objectsCount -= currentLevelCount;
        }
    }

    private int CalculateLevelObjectCount(int currentLevel, int maxLevel, int remainingCount, List<int> previousCounts, Random random)
    {
        int count = currentLevel == maxLevel ? remainingCount : (int)(remainingCount / (random.NextDouble() * (3 - 1.8) + 1.8));
        count = AdjustCountBasedOnPreviousSum(previousCounts, count);
        previousCounts.Add(count);
        return count;
    }
    
    private int AdjustCountBasedOnPreviousSum(List<int> previousCounts, int currentCount)
    {
        if (previousCounts.Sum() % 2 == 0)
        {
            return currentCount % 2 == 0 ? currentCount : currentCount + 1;
        }
        else
        {
            return currentCount % 2 == 0 ? currentCount + 1 : currentCount;
        }
    }
}