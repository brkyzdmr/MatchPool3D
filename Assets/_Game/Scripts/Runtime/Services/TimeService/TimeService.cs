using System;
using UnityEngine;

public class TimeService : Service, ITimeService
{
    private readonly ITimeProvider _timeProvider;

    private int _lastCachedNow = -1;
    private int _lastCachedUtcNow = -1;

    private DateTime _cachedNow;
    private DateTime _cachedUtcNow;
    private readonly Contexts _contexts;

    public TimeService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
        _timeProvider = new UnityTimeProvider();
    }

    public float TimerSpeedFactor => _contexts.config.gameConfig.value.GameConfig.timerSpeedFactor;
    public float TimerSpeedFactorMax => _contexts.config.gameConfig.value.GameConfig.timerSpeedFactorMax;

    public DateTime Now
    {
        get
        {
            CacheCurrentTime();
            return _cachedNow;
        }
    }

    public DateTime UtcNow
    {
        get
        {
            CacheCurrentUtcTime();
            return _cachedUtcNow;
        }
    }

    private void CacheCurrentTime()
    {
        if (Time.frameCount != _lastCachedNow)
        {
            _cachedNow = _timeProvider.Now;
            _lastCachedNow = Time.frameCount;
        }
    }

    private void CacheCurrentUtcTime()
    {
        if (Time.frameCount != _lastCachedUtcNow)
        {
            _cachedUtcNow = _timeProvider.UtcNow;
            _lastCachedUtcNow = Time.frameCount;
        }
    }

    public float DeltaTime => _timeProvider.DeltaTime;

    public double RealtimeSinceStartup => _timeProvider.RealtimeSinceStartup;

    public float TimeScale
    {
        get => _timeProvider.TimeScale;
        set => _timeProvider.TimeScale = value;
    }

    public bool IsTimePaused { get; private set; }

    public void PauseTime()
    {
        IsTimePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeTime()
    {
        IsTimePaused = false;
        Time.timeScale = 1f;
    }

    public string FormatTimeDuration(float timeInSeconds)
    {
        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds % 60;
        return $"{minutes:00}:{seconds:00}";
    }

    public void SetTimerSpeedFactor(float value)
    {
        _contexts.timer.ReplaceTimerSpeed(value);
    }
}