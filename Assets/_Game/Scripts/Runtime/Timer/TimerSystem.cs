using Entitas;
using UnityEngine;

public sealed class TimerSystem : IExecuteSystem
{
    private readonly TimerContext timer;
    private readonly IGroup<TimerEntity> runningTimers;

    public TimerSystem(Contexts contexts)
    {
        timer = contexts.timer;
        runningTimers = timer.GetGroup(
            TimerMatcher.AllOf(
                TimerMatcher.Timer,
                TimerMatcher.TimerRunning
            )
        );
    }

    public void Execute()
    {
        var delta = Time.deltaTime;
        foreach (var e in runningTimers.GetEntities())
        {
            e.timer.Remaining -= delta;

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

    public void ChangeRemainingTime(TimerEntity e, float newTime)
    {
        e.timer.Remaining = newTime;
    }
}