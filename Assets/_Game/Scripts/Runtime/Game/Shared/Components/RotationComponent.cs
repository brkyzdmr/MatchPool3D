using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Event(EventTarget.Self)]
public sealed class RotationComponent : IComponent
{
    public Vector3 Value;
}