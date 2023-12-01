using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique, Event(EventTarget.Any)]
public class RemainingObjectsCountComponent : IComponent
{
    public int Value;
}
