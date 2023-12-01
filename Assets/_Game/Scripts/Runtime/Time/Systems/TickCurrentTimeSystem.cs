using Entitas;
using UnityEngine;

public sealed class TickCurrentTimeSystem : IExecuteSystem, IInitializeSystem
{
    readonly Contexts _contexts;
    double _lastTick;

    static int _cheatTime;
    public static void AdvanceCheatTime() => _cheatTime += 60;
    static long CalculateTotalSeconds() => TimeService.Instance.Now.ToEpochUnixTimestamp() + _cheatTime;

    public TickCurrentTimeSystem(Contexts contexts)
    {
        _contexts = contexts;
        _lastTick = Time.realtimeSinceStartupAsDouble;
    }

    public void Initialize()
    {
        _contexts.game.SetCurrentTime(CalculateTotalSeconds());
    }

    public void Execute()
    {
        if (Time.realtimeSinceStartupAsDouble - 1.0f >= _lastTick)
        {
            _contexts.game.ReplaceCurrentTime(CalculateTotalSeconds());
            _lastTick = Time.realtimeSinceStartupAsDouble;
            _contexts.game.CreateEntity().isTimeTick = true;
        }
    }
}
