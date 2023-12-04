
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Event(EventTarget.Self)]
public sealed class ObjectComponent : IComponent
{
    public string Type;
    public int Level;
}
