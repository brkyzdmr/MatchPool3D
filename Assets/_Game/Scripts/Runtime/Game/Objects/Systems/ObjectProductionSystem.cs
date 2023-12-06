
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

        var objectPath = _objectService.GetObjectPath(nextAvailableObject.Value, nextAvailableObject.Key);
        _contexts.game.CreateObject(nextAvailableObject.Value.type, nextAvailableObject.Key, objectPath, randomPosition);

        UpdateObjectCounters();
    }

    private Vector3 GetRandomPosition() => 
        new Vector3(Random.Range(-3, 3), 6, Random.Range(-5, 5));

    private KeyValuePair<int, ObjectsConfigData.ObjectData> GetNextAvailableObject()
    {
        var objects = _contexts.game.objectsWillProduced.Objects;

        if (!objects.Any())
        {
            throw new InvalidOperationException("No available objects to produce.");
        }

        _lastProducedObjectIndex = (_lastProducedObjectIndex + 1) % objects.Count; 

        return objects.ElementAt(_lastProducedObjectIndex);
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
