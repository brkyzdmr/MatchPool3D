//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class TimerContext {

    public TimerEntity timerSpeedEntity { get { return GetGroup(TimerMatcher.TimerSpeed).GetSingleEntity(); } }
    public TimerSpeedComponent timerSpeed { get { return timerSpeedEntity.timerSpeed; } }
    public bool hasTimerSpeed { get { return timerSpeedEntity != null; } }

    public TimerEntity SetTimerSpeed(float newValue) {
        if (hasTimerSpeed) {
            throw new Entitas.EntitasException("Could not set TimerSpeed!\n" + this + " already has an entity with TimerSpeedComponent!",
                "You should check if the context already has a timerSpeedEntity before setting it or use context.ReplaceTimerSpeed().");
        }
        var entity = CreateEntity();
        entity.AddTimerSpeed(newValue);
        return entity;
    }

    public void ReplaceTimerSpeed(float newValue) {
        var entity = timerSpeedEntity;
        if (entity == null) {
            entity = SetTimerSpeed(newValue);
        } else {
            entity.ReplaceTimerSpeed(newValue);
        }
    }

    public void RemoveTimerSpeed() {
        timerSpeedEntity.Destroy();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class TimerEntity {

    public TimerSpeedComponent timerSpeed { get { return (TimerSpeedComponent)GetComponent(TimerComponentsLookup.TimerSpeed); } }
    public bool hasTimerSpeed { get { return HasComponent(TimerComponentsLookup.TimerSpeed); } }

    public void AddTimerSpeed(float newValue) {
        var index = TimerComponentsLookup.TimerSpeed;
        var component = (TimerSpeedComponent)CreateComponent(index, typeof(TimerSpeedComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTimerSpeed(float newValue) {
        var index = TimerComponentsLookup.TimerSpeed;
        var component = (TimerSpeedComponent)CreateComponent(index, typeof(TimerSpeedComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTimerSpeed() {
        RemoveComponent(TimerComponentsLookup.TimerSpeed);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class TimerMatcher {

    static Entitas.IMatcher<TimerEntity> _matcherTimerSpeed;

    public static Entitas.IMatcher<TimerEntity> TimerSpeed {
        get {
            if (_matcherTimerSpeed == null) {
                var matcher = (Entitas.Matcher<TimerEntity>)Entitas.Matcher<TimerEntity>.AllOf(TimerComponentsLookup.TimerSpeed);
                matcher.componentNames = TimerComponentsLookup.componentNames;
                _matcherTimerSpeed = matcher;
            }

            return _matcherTimerSpeed;
        }
    }
}