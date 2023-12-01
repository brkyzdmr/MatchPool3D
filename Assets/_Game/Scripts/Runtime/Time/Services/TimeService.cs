using System;
using UnityEngine;

public partial class TimeService
{
    public static TimeService Instance
    {
        get
        {
            _instance ??= new TimeService(new UnityTimeProvider());
            return _instance;
        }
    }
    static TimeService _instance;

    const string D = "d";
    const string H = "h";
    const string M = "m";
    const string S = "s";

    readonly ITimeProvider _timeProvider;

    int _lastCachedNow = -1;
    int _lastCachedUtcNow = -1;

    DateTime _cachedNow;
    DateTime _cachedUtcNow;

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

    public string GetReadableTimeSpan(DateTime startDate, DateTime endDate)
    {
        var timeSpan = endDate - startDate;
        if (timeSpan.TotalDays >= 1) return (int)timeSpan.TotalDays + D + " " + timeSpan.Hours + H;
        if (timeSpan.TotalHours >= 1) return (int)timeSpan.TotalHours + H + " " + timeSpan.Minutes + M;
        if (timeSpan.TotalMinutes >= 1) return (int)timeSpan.TotalMinutes + M + " " + timeSpan.Seconds + S;

        return Math.Max(0, timeSpan.Seconds) + S;
    }
}
