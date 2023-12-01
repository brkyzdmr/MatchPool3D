using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Event(EventTarget.Self)]
public sealed class ScaleComponent : IComponent
{
    public Vector3 Value;
}