using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class CurrentLevelIndexComponent : IComponent
{
    public int Value;
}