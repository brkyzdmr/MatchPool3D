using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class MaxProducedObjectCountComponent : IComponent
{
    public int Value;
}