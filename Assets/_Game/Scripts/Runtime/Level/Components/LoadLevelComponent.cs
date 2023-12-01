using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique, Cleanup(CleanupMode.DestroyEntity)]
public sealed class LoadLevelComponent : IComponent
{

}

