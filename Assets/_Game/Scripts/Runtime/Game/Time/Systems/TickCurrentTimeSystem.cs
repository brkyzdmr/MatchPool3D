using Entitas;
using UnityEngine;

public sealed class TickCurrentTimeSystem : IExecuteSystem, IInitializeSystem
{
    private readonly Contexts _contexts;
    private double _lastTick;
    private GameEntity _lastTickEntity;
    private ITimeService _timeService;

    private long CalculateTotalSeconds() => _timeService.Now.ToEpochUnixTimestamp();

    public TickCurrentTimeSystem(Contexts contexts)
    {
        _contexts = contexts;
        _timeService = Services.GetService<ITimeService>();
        _lastTick = _timeService.RealtimeSinceStartup;
    }

    public void Initialize()
    {
        _contexts.game.SetCurrentTime(CalculateTotalSeconds());
    }

    public void Execute()
    {
        if (_timeService.IsTimePaused) { return; }
        
        if (_timeService.RealtimeSinceStartup - 1.0f >= _lastTick)
        {
            _contexts.game.ReplaceCurrentTime(CalculateTotalSeconds());
            _lastTick = _timeService.RealtimeSinceStartup;
            
            if (_lastTickEntity != null && !_lastTickEntity.isDestroyed)
            {
                _lastTickEntity.Destroy();
            }
            
            _lastTickEntity = _contexts.game.CreateEntity();
            _lastTickEntity.isTimeTick = true;
        }
    }
}