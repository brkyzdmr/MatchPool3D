using System;

public interface ITimeService
{
    public float TimerSpeedFactor { get; }
    public float TimerSpeedFactorMax { get; }
    public DateTime Now { get; }
    public DateTime UtcNow { get; }
    public float DeltaTime { get; }
    public double RealtimeSinceStartup { get; }
    public float TimeScale { get; set; }
    public bool IsTimePaused { get; }
    public void PauseTime();
    public void ResumeTime();
    public string FormatTimeDuration(float timeInSeconds);
    public void SetTimerSpeedFactor(float value);
}