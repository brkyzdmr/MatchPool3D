using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class CreatedObjectsCountComponent : IComponent
{
    public int Value;
}

