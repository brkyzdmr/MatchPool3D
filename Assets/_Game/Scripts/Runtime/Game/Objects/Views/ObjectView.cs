using System;
using Entitas;
using UnityEngine;

public class ObjectView : View, 
    IRigidbodyListener, 
    IColliderListener
{
    private MeshRenderer _renderer;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private GameConfigData _gameConfig;

    public override void Link(Contexts contexts,IEntity entity)
    {
        base.Link(contexts,entity);
        _gameConfig = contexts.config.gameConfig.value.GameConfig;
        _linkedEntity.AddRigidbodyListener(this);
        _linkedEntity.AddColliderListener(this);

        _renderer = GetComponentInChildren<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();
        
        SetPhysicsData();
    }

    private void OnCollisionEnter(Collision other)
    {
        var otherObject = other.transform.GetComponentInParent<View>();
        if (otherObject == null)
        {
            return;
        }

        GameEntity otherEntity = otherObject.LinkedEntity;

        if (otherEntity != null && _linkedEntity.isEnabled) 
        {
            _linkedEntity.ReplaceCollision(otherEntity, other.contacts[0].point, other.relativeVelocity);
        }
    }

    public void OnRigidbody(GameEntity entity, bool isKinematic, Vector3 velocity)
    {
        _rigidbody.isKinematic = isKinematic;
        _rigidbody.velocity = velocity;
    }

    public void OnCollider(GameEntity entity, bool isEnabled, bool isTrigger)
    {
        _collider.enabled = isEnabled;
        _collider.isTrigger = isTrigger;
    }

    private void SetPhysicsData()
    {
        var objectMass = _gameConfig.objectsMass;
        var objectBounciness = _gameConfig.objectsBounciness;

        _rigidbody.mass = objectMass;
        _collider.material.bounciness = objectBounciness;
    }
}