using Entitas;
using UnityEngine;

public class ApplyForceSystem : IExecuteSystem {
    private readonly IGroup<GameEntity> _rigidbodies;

    public ApplyForceSystem(Contexts contexts) {
        _rigidbodies = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Rigidbody, GameMatcher.Force));
    }

    public void Execute() {
        foreach(var e in _rigidbodies.GetEntities())
        {
            var force = e.force.Force;
            var mass = e.rigidbody.Mass;
            e.rigidbody.Velocity += force / mass * Time.deltaTime;
            e.RemoveForce(); // Assuming force is a one-time application
        }
    }
}