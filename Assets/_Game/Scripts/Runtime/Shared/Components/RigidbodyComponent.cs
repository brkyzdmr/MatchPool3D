using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Event(EventTarget.Self)]
public sealed class RigidbodyComponent : IComponent
{
    public bool IsKinematic;
    public Vector3 Velocity;
}