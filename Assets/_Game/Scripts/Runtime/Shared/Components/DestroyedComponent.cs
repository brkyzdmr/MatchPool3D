using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Event(EventTarget.Self), Cleanup(CleanupMode.DestroyEntity)]
public sealed class DestroyedComponent : IComponent
{
}