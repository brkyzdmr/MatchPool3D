using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class LevelDurationComponent : IComponent
{
    public int Value;
}
