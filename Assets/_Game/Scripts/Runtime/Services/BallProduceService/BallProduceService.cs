
using UnityEngine;

public class BallProduceService : Service
{
    public BallProduceService(Contexts contexts) : base(contexts)
    {
    }

    public GameEntity CreateBall(int id, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        var entity = _contexts.game.CreateEntity();
        entity.AddId(id);
        entity.AddPosition(position);
        entity.AddRotation(rotation);
        entity.AddScale(scale);
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
        entity.AddAsset("Ball");

        return entity;
    }
}