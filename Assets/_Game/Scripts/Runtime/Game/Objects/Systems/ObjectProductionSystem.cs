
using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectProductionSystem : IInitializeSystem, IExecuteSystem
{
    private readonly TimerContext _timerContext;
    private TimerEntity _productionTimer;
    private readonly Contexts _contexts;
    private readonly GameContext _context;
    private const float TimerInterval = 2f;
    private readonly ILevelService _levelService;
    private readonly IObjectService _objectService;
    private int _lastProducedObjectIndex = -1; 
    
    public ObjectProductionSystem(Contexts contexts)
    {
        _contexts = contexts;
        _timerContext = contexts.timer;
        _levelService = Services.GetService<ILevelService>();
        _objectService = Services.GetService<IObjectService>();
    }

    public void Initialize()
    {
        _productionTimer = _timerContext.CreateEntity();
        _productionTimer.AddTimer(TimerInterval);
        _productionTimer.isTimerRunning = true;
    }

    public void Execute()
    {
        if (ShouldProduceObject())
        {
            ProduceObject();
            ResetProductionTimer();
        }
    }

    private void ProduceObject()
    {
        var randomPosition = GetRandomPosition();
        var nextAvailableObject = GetNextAvailableObject();

        var objectPath = _objectService.GetObjectPath(nextAvailableObject.Item1, nextAvailableObject.Item2);
        _contexts.game.CreateObject(nextAvailableObject.Item1.type, nextAvailableObject.Item2, objectPath, randomPosition);

        UpdateObjectCounters();
    }

    private Vector3 GetRandomPosition() => 
        new Vector3(Random.Range(-3, 3), 6, Random.Range(-5, 5));

    // TODO: Refactor this
    private (ObjectsConfigData.ObjectData, int) GetNextAvailableObject()
    {
        var objects = _contexts.game.generatedObjects.GeneratedObjects;

        // Filter the objects that have a non-zero count
        var availableObjects = objects.Where(obj => obj.Item3 > 0).ToList();

        // If no available objects, handle the situation (e.g., throw exception or return a default value)
        if (!availableObjects.Any())
        {
            throw new InvalidOperationException("No available objects.");
        }

        // Randomly select an object from the available ones
        var rndValue = Rand.game.Int(availableObjects.Count);
        var selectedObject = availableObjects[rndValue];

        // Decrease the objectCount of the selected object
        int objectIndex = objects.IndexOf(selectedObject); // fix it
        objects[objectIndex] = (selectedObject.Item1, selectedObject.Item2, selectedObject.Item3 - 1);

        // Return the ObjectData and level of the selected object
        return (selectedObject.Item1, selectedObject.Item2);
    }


    private void UpdateObjectCounters()
    {
        _contexts.game.ReplaceCreatedObjectsCount(_contexts.game.createdObjectsCount.Value + 1);
        _contexts.game.ReplaceRemainingObjectsCount(_contexts.game.remainingObjectsCount.Value + 1);
    }

    private bool ShouldProduceObject() =>
        _contexts.game.createdObjectsCount.Value < _contexts.game.maxProducedObjectCount.Value &&
        !_productionTimer.isTimerRunning;

    private void ResetProductionTimer()
    {
        _productionTimer.ReplaceTimer(TimerInterval);
        _productionTimer.isTimerRunning = true;
        _lastProducedObjectIndex = -1; 
    }
}
