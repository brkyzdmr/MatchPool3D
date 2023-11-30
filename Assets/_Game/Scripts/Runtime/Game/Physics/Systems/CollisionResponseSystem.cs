using Entitas;
using UnityEngine;

public class CollisionResponseSystem : IExecuteSystem 
{
    private readonly IGroup<GameEntity> _collidingEntities;

    public CollisionResponseSystem(Contexts contexts) {
        _collidingEntities = contexts.game.GetGroup(GameMatcher.Colliding);
    }

    public void Execute() {
        foreach (var collidingEntity in _collidingEntities.GetEntities()) {
            var collision = collidingEntity.collision;
            // Reflect the velocity vector based on collision normal and bounciness
            collidingEntity.rigidbody.Velocity = Vector3.Reflect(collidingEntity.rigidbody.Velocity, 
                collision.CollisionNormal) * collision.Bounciness;
            collidingEntity.RemoveCollision();
        }
    }
}