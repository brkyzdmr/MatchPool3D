using Entitas;
using Entitas.Unity;
using UnityEngine;

public class View : MonoBehaviour, 
    IView, 
    IPositionListener, 
    IRotationListener, 
    IDestroyedListener, 
    IQuaternionRotationListener, 
    IScaleListener
{
    public GameEntity LinkedEntity => _linkedEntity;

    protected GameEntity _linkedEntity;
    protected Contexts _contexts;
    
    public virtual void Link(Contexts contexts, IEntity entity)
    {
        _contexts = contexts;
        gameObject.Link(entity);
        _linkedEntity = (GameEntity)entity;
        _linkedEntity.AddPositionListener(this);
        _linkedEntity.AddRotationListener(this);
        _linkedEntity.AddQuaternionRotationListener(this);
        _linkedEntity.AddScaleListener(this);
        _linkedEntity.AddDestroyedListener(this);

        var pos = Vector3.zero;
        if (_linkedEntity.hasPosition)
        {
            pos = _linkedEntity.position.Value;
            transform.position = pos;
        }
    }
    
    public virtual void OnPosition(GameEntity entity, Vector3 value) => transform.localPosition = value;

    public void OnRotation(GameEntity entity, Vector3 value) => transform.localRotation = Quaternion.Euler(value.x, value.y, value.z);

    public void OnQuaternionRotation(GameEntity entity, Quaternion value) => transform.localRotation = value;

    public void OnScale(GameEntity entity, Vector3 value) => transform.localScale = value;

    public virtual void OnDestroyed(GameEntity entity) => OnDestroy();

    protected virtual void OnDestroy()
    {
        if (gameObject.GetEntityLink()?.entity != null)
            gameObject.Unlink();
        Destroy(gameObject);
    }
}
