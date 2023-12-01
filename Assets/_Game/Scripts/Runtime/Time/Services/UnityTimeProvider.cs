using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("Game", "GK0001:Do not use DateTime")]
public partial class UnityTimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public float DeltaTime => Time.deltaTime;

    public float TimeScale
    {
        get => Time.timeScale;
        set => Time.timeScale = value;
    }
}
