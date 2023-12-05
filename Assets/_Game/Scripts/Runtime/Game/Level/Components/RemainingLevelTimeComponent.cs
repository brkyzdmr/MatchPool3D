
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique, Event(EventTarget.Any)]
public class RemainingLevelTimeComponent : IComponent
{
    public int Value;
}