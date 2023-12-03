
using UnityEngine;

public static class ObjectExtension
{
    public static GameEntity CreateObject(this GameContext context, string path, Vector3 position)
    {
        var entity = context.CreateEntity();
        entity.AddAsset(path);
        entity.AddPosition(position);
        entity.AddRigidbody(false, Vector3.zero);
        entity.isMergableObject = true;
        return entity;
    }
}
