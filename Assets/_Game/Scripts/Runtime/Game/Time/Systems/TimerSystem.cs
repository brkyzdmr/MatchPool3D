using Entitas;
using UnityEngine;

public sealed class TimerSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private readonly IGroup<TimerEntity> _runningTimers;
    private readonly ITimeService _timeService;

    public TimerSystem(Contexts contexts)
    {
        _contexts = contexts;
        _timeService = Services.GetService<ITimeService>();

        _runningTimers = _contexts.timer.GetGroup(
            TimerMatcher.AllOf(
                TimerMatcher.Timer,
                TimerMatcher.TimerRunning
            )
        );
    }

    public void Execute()
    {
        var delta = _timeService.DeltaTime;

        foreach (var e in _runningTimers.GetEntities())
        {
            e.timer.Remaining -= delta * _contexts.timer.timerSpeed.Value;

            if (e.timer.Remaining <= 0.0f)
            {
                e.timer.Remaining = 0.0f;
                e.isTimerRunning = false;

                if (e.willDestroyWhenTimerExpires)
                {
                    e.Destroy();
                }
            }
        }
    }
}