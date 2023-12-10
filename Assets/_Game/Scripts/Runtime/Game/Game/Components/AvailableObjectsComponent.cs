using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class AvailableObjectsComponent : IComponent
{
    public int Value;
}