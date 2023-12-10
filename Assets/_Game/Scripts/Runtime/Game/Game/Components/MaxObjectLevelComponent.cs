using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class MaxObjectLevelComponent : IComponent
{
    public int Value;
}