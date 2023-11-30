using Entitas;
using UnityEngine;

[Game]
public class RigidbodyComponent : IComponent {
    public Vector3 Velocity;
    public float Mass;
    public bool UseGravity;
    public bool IsKinematic;
}