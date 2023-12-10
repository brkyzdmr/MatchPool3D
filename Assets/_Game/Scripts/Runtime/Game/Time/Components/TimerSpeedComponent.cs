
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique, Timer, Event(EventTarget.Any)]
public class TimerSpeedComponent : IComponent
{
    public float Value;
}
