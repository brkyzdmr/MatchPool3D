
using Entitas;
using UnityEngine;

public class ObjectProductionSystem : IInitializeSystem, IExecuteSystem
{
    private readonly TimerContext _timerContext;
    private TimerEntity _productionTimer;
    private readonly Contexts _contexts;
    private readonly GameContext _context;
    private int _createdObjectCount = 0;
    private float _timerInteval = 2f;
    
    public ObjectProductionSystem(Contexts contexts)
    {
        _contexts = contexts;
        _context = contexts.game;
        _timerContext = contexts.timer;
    }

    public void Initialize()
    {
        // Set up the initial production timer
        _productionTimer = _timerContext.CreateEntity();
        _productionTimer.AddTimer(_timerInteval); // Interval for producing new balls
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
        var randomAvailableObjectPath = ObjectService.GetRandomAvailableObjectPath(1);
        _contexts.game.CreateObject(randomAvailableObjectPath, randomPosition);
        _createdObjectCount += 1;
        _contexts.game.ReplaceCreatedObjectsCount(_createdObjectCount);
    }

    private bool ShouldProduceObject()
    {
        return _createdObjectCount != LevelService.MaxProducedObjectCount && !_productionTimer.isTimerRunning;
    }
    
    private void ResetProductionTimer()
    {
        _productionTimer.ReplaceTimer(_timerInteval);

        _productionTimer.isTimerRunning = true;
    }
}
