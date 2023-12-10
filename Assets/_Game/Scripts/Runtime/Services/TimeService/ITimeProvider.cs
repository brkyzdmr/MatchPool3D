using System;

public interface ITimeProvider
{
    public DateTime Now { get; }
    public DateTime UtcNow { get; }
    public float DeltaTime { get; }
    public double RealtimeSinceStartup { get; }
    public float TimeScale { get; set; }
}
