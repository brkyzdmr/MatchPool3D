
using Entitas;
using UnityEngine;

public class ObjectProductionSystem : IInitializeSystem, IExecuteSystem
{
    private readonly TimerContext _timerContext;
    private TimerEntity _productionTimer;
    private readonly Contexts _contexts;
    private readonly GameContext _context;
    private float _timerInteval = 2f;
    private readonly ILevelService _levelService;
    private readonly IObjectService _objectService;

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
        var randomAvailableObject = _objectService.GetRandomAvailableObject(_levelService.AvailableObjects);
        var randomAvailableObjectPath = _objectService.GetObjectPath(randomAvailableObject, 1);

        _contexts.game.CreateObject(randomAvailableObject.type, 1, randomAvailableObjectPath, randomPosition);
        _levelService.CreatedObjectCount += 1;
        _contexts.game.ReplaceCreatedObjectsCount(_levelService.CreatedObjectCount);
        _contexts.game.ReplaceRemainingObjectsCount(_contexts.game.remainingObjectsCount.Value + 1);
    }

    private bool ShouldProduceObject()
    {
        return _levelService.CreatedObjectCount != _levelService.MaxProducedObjectCount && !_productionTimer.isTimerRunning;
    }
    
    private void ResetProductionTimer()
    {
        _productionTimer.ReplaceTimer(_timerInteval);

        _productionTimer.isTimerRunning = true;
    }
}
