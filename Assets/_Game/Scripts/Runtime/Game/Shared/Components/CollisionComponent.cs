using Entitas;
using UnityEngine;

[Game]
public sealed class CollisionComponent : IComponent
{
    public GameEntity OtherEntity;
    public Vector3 CollisionPoint;
    public Vector3 RelativeVelocity; 
}