using Entitas;
using UnityEngine;

public class PhysicsUpdateSystem : IExecuteSystem {
    private readonly IGroup<GameEntity> _rigidbodies;

    public PhysicsUpdateSystem(Contexts contexts) {
        _rigidbodies = contexts.game.GetGroup(GameMatcher.Rigidbody);
    }

    public void Execute() {
        foreach(var e in _rigidbodies.GetEntities()) {
            if (e.rigidbody.UseGravity) {
                e.rigidbody.Velocity += Physics.gravity * Time.deltaTime;
            }
            
            var newPosition = e.position.Value + e.rigidbody.Velocity * Time.deltaTime;
            e.ReplacePosition(newPosition);
        }
    }
}