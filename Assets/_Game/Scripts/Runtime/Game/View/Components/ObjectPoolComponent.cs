using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class ObjectPoolComponent : IComponent
{
    [PrimaryEntityIndex]
    public string Id;

    public ObjectPool<IView> Value;
}

