using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class LevelNameComponent : IComponent
{
    public string Value;
}