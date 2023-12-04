using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique, Event(EventTarget.Any)]
public sealed class CurrentMergedObjectsCount : IComponent
{
    public int Value;
}
