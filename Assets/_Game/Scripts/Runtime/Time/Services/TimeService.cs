using System;
using UnityEngine;

public class TimeService
{
    public static TimeService Instance
    {
        get
        {
            _instance ??= new TimeService(new UnityTimeProvider());
            return _instance;
        }
    }
    private static TimeService _instance;

    private readonly ITimeProvider _timeProvider;

    private int _lastCachedNow = -1;
    private int _lastCachedUtcNow = -1;

    private DateTime _cachedNow;
    private DateTime _cachedUtcNow;

    public TimeService(ITimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public DateTime Now
    {
        get
        {
            if (Time.frameCount != _lastCachedNow)
            {
                _cachedNow = _timeProvider.Now;
                _lastCachedNow = Time.frameCount;
            }

            return _cachedNow;
        }
    }

    public DateTime UtcNow
    {
        get
        {
            if (Time.frameCount != _lastCachedUtcNow)
            {
                _cachedUtcNow = _timeProvider.UtcNow;
                _lastCachedUtcNow = Time.frameCount;
            }

            return _cachedUtcNow;
        }
    }

    public float DeltaTime => _timeProvider.DeltaTime;

    public float TimeScale
    {
        get => _timeProvider.TimeScale;
        set => _timeProvider.TimeScale = value;
    }
    
    public static string FormatTimeDuration(float timeInSeconds)
    {
        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds % 60;
        return $"{minutes:00}:{seconds:00}";
    }
}
