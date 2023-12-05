using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class CurrentLevelNameComponent : IComponent
{
    public string Value;
}