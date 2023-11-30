using Entitas;
using UnityEngine;

[Game]
public class CollisionComponent : IComponent {
    public GameEntity OtherEntity; // Reference to the other entity in the collision
    public Vector3 CollisionNormal; // The normal vector of the collision surface
    public float Bounciness; // Bounciness factor
}