
using UnityEngine;

public static class ObjectExtension
{
    public static GameEntity CreateObject(this GameContext context, Vector3 position)
    {
        var entity = context.CreateEntity();
        entity.AddAsset("Prefabs/Ball");
        entity.AddPosition(position);
        entity.isMergableObject = true;
        return entity;
    }
}
