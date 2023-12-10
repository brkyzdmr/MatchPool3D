using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class MaxProducedObjectLevelComponent : IComponent
{
    public int Value;
}