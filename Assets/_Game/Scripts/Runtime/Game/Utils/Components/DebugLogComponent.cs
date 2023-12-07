using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique] 
public sealed class DebugLogComponent : IComponent 
{
    public string Message;
}