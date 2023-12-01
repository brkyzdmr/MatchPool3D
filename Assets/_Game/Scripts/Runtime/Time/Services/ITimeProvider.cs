using System;

public partial interface ITimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    float DeltaTime { get; }
    float TimeScale { get; set; }
}
