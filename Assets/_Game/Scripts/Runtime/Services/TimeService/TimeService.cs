using UnityEngine;

public sealed class TimeService : Service, ITimeService
{
    public TimeService(Contexts contexts) : base(contexts)
    {
    }
    
    public float FixedDeltaTime()
    {
        return Time.fixedDeltaTime;
    }

    public float DeltaTime()
    {
        return Time.deltaTime;
    }

    public float RealtimeSinceStartup()
    {
        return Time.realtimeSinceStartup;
    }
}