
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique, Event(EventTarget.Any)]
public sealed class LevelStatusComponent : IComponent
{
    public LevelStatus Value;
}
