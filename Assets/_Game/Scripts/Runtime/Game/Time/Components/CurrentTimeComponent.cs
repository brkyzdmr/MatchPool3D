using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public sealed class CurrentTimeComponent : IComponent
{
    public long Value;
}

