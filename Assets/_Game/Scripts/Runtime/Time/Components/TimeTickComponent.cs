using Entitas;
using Entitas.CodeGeneration.Attributes;

[Event(EventTarget.Any),Cleanup(CleanupMode.DestroyEntity)]
public sealed class TimeTickComponent : IComponent
{
}

