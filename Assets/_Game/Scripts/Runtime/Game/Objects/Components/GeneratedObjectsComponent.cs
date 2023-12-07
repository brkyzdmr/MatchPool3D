
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique]
public sealed class GeneratedObjectsComponent : IComponent
{
    public List<(ObjectsConfigData.ObjectData, int, int)> GeneratedObjects;
}
