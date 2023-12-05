using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class TotalGoldComponent : IComponent
{
    public int Value;
}