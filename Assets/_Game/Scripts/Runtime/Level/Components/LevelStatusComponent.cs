
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique, Event(EventTarget.Any)]
public class LevelStatusComponent : IComponent
{
    public LevelStatus Value;
}
