using Entitas;
using UnityEngine;

public class ObjectView : View, 
    IRigidbodyListener, 
    IColliderListener
{
    [SerializeField] private new MeshRenderer renderer;
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Collider collider;
    public override void Link(Contexts contexts,IEntity entity)
    {
        base.Link(contexts,entity);
        _linkedEntity.AddRigidbodyListener(this);
        _linkedEntity.AddColliderListener(this);
    }

    public void OnRigidbody(GameEntity entity, bool isKinematic, Vector3 velocity)
    {
        rigidbody.isKinematic = isKinematic;
        rigidbody.velocity = velocity;
    }

    public void OnCollider(GameEntity entity, bool isEnabled, bool isTrigger)
    {
        collider.enabled = isEnabled;
        collider.isTrigger = isTrigger;
    }
}