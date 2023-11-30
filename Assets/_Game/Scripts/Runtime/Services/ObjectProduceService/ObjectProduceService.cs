
using UnityEngine;

public class ObjectProduceService : Service
{
    public ObjectProduceService(Contexts contexts) : base(contexts)
    {
    }

    public GameEntity CreateBall(int id, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        var entity = _contexts.game.CreateEntity();
        entity.AddId(id);
        entity.AddPosition(position);
        entity.AddRotation(rotation);
        entity.AddScale(scale);
        entity.AddRigidbody(Vector3.zero, 1, true, false);
        entity.AddRadius(1);
        entity.AddBounciness(0.3f);
        entity.AddAsset("Ball");

        return entity;
    }
    
    public GameEntity CreateBall(int id, Vector3 position)
    {
        var entity = _contexts.game.CreateEntity();
        entity.AddId(id);
        entity.AddPosition(position);
        entity.AddRotation(Quaternion.identity);
        entity.AddScale(Vector3.one);
        entity.AddRigidbody(Vector3.zero, 1, true, false);
        entity.AddRadius(1);
        entity.AddBounciness(0.3f);
        entity.AddAsset("Ball");

        return entity;
    }

    public GameEntity CreatePool(int id, Vector3 position)
    {
        var entity = _contexts.game.CreateEntity();
        entity.AddId(id);
        entity.AddPosition(position);
        entity.AddRigidbody(Vector3.zero, 1, false, false);
        entity.AddAsset("Pool");
        return entity;
    }
}