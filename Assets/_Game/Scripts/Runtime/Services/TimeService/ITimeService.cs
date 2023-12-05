using System;

public interface ITimeService
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    float DeltaTime { get; }
    float TimeScale { get; set; }
    bool IsTimePaused { get; }
    void PauseTime();
    void ResumeTime();
    string FormatTimeDuration(float timeInSeconds);
}