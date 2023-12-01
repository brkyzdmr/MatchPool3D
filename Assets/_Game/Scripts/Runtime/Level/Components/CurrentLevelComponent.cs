using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique, Event(EventTarget.Any), Event(EventTarget.Any, EventType.Removed)]
public sealed class CurrentLevelComponent : IComponent
{
    public int Value;
}
