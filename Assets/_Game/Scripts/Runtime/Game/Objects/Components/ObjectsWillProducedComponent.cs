
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique]
public class ObjectsWillProducedComponent : IComponent
{
    public Dictionary<int, ObjectsConfigData.ObjectData> Objects;
}
