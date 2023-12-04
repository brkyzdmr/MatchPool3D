
using Entitas;
using UnityEngine;

public class ObjectProductionSystem : IInitializeSystem, IExecuteSystem
{
    private readonly TimerContext _timerContext;
    private TimerEntity _productionTimer;
    private readonly Contexts _contexts;
    private readonly GameContext _context;
    private float _timerInteval = 2f;
    private readonly Services _services;

    public ObjectProductionSystem(Contexts contexts, Services services)
    {
        _contexts = contexts;
        _services = services;
        _timerContext = contexts.timer;
    }

    public void Initialize()
    {
        _productionTimer = _timerContext.CreateEntity();
        _productionTimer.AddTimer(_timerInteval);
        _productionTimer.isTimerRunning = true;
    }

    public void Execute()
    {
        if (ShouldProduceObject())
        {
            CreateNewObject();
            
            ResetProductionTimer();
        }
    }

    private void CreateNewObject()
    {
        var randomPosition = new Vector3(Random.Range(-3, 3), 10, Random.Range(-5, 5));
        var randomAvailableObject = _services.ObjectService.GetRandomAvailableObject(_services.LevelService.AvailableObjects);
        var randomAvailableObjectPath = _services.ObjectService.GetObjectPath(randomAvailableObject, 1);

        _contexts.game.CreateObject(randomAvailableObject.type, 1, randomAvailableObjectPath, randomPosition);
        _services.LevelService.CreatedObjectCount += 1;
        _contexts.game.ReplaceCreatedObjectsCount(_services.LevelService.CreatedObjectCount);
        _contexts.game.ReplaceRemainingObjectsCount(_contexts.game.remainingObjectsCount.Value + 1);
    }

    private bool ShouldProduceObject()
    {
        return _services.LevelService.CreatedObjectCount != _services.LevelService.MaxProducedObjectCount && !_productionTimer.isTimerRunning;
    }
    
    private void ResetProductionTimer()
    {
        _productionTimer.ReplaceTimer(_timerInteval);

        _productionTimer.isTimerRunning = true;
    }
}
