using Entitas;
using UnityEngine;

public class ObjectView : View, 
    IRigidbodyListener, 
    IColliderListener
{
    private MeshRenderer _renderer;
    private Rigidbody _rigidbody;
    private Collider _collider;
    public override void Link(Contexts contexts,IEntity entity)
    {
        base.Link(contexts,entity);
        _linkedEntity.AddRigidbodyListener(this);
        _linkedEntity.AddColliderListener(this);

        _renderer = GetComponentInChildren<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();
    }

    public void OnRigidbody(GameEntity entity, bool isKinematic, Vector3 velocity)
    {
        _rigidbody.isKinematic = isKinematic;
        _rigidbody.velocity = velocity;
        
        Debug.Log("isKinematic: " + isKinematic + ", Velocity: " + velocity);
    }

    public void OnCollider(GameEntity entity, bool isEnabled, bool isTrigger)
    {
        _collider.enabled = isEnabled;
        _collider.isTrigger = isTrigger;
    }
}