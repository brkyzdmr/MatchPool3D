using Entitas;
using UnityEngine;

public class CollisionDetectionSystem : IExecuteSystem {
    private readonly IGroup<GameEntity> _entitiesWithColliders;

    public CollisionDetectionSystem(Contexts contexts) {
        _entitiesWithColliders = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.Rigidbody)
            .AnyOf(GameMatcher.BoxCollider, GameMatcher.SphereCollider));
    }

    public void Execute() {
        var entities = _entitiesWithColliders.GetEntities();
        for (int i = 0; i < entities.Length; i++) {
            var a = entities[i];
            
            for (int j = i + 1; j < entities.Length; j++)
            {
                var b = entities[j];
                if (CheckCollision(a, b, out var collisionNormal)) {
                    a.isColliding = true; 
                    b.isColliding = true;
                    
                    if (!a.hasCollision)
                    {
                        a.AddCollision(b, collisionNormal, a.bounciness.Value);
                    }

                    if (!b.hasCollision)
                    {
                        b.AddCollision(a, -collisionNormal, b.bounciness.Value);
                    }
                }
            }
        }
    }

    private bool CheckCollision(GameEntity entity1, GameEntity entity2, out Vector3 collisionNormal)
    {
        collisionNormal = Vector3.zero;
        
        if (entity1.hasBoxCollider && entity2.hasBoxCollider) 
        {
            return AABBvsAABB(entity1.boxCollider, entity1.position.Value, entity2.boxCollider, 
                entity2.position.Value, out collisionNormal);
        }  
        
        if (entity1.hasBoxCollider && entity2.hasSphereCollider) 
        {
            return AABBvsSphere(entity1.boxCollider, entity1.position.Value, entity2.sphereCollider, 
                entity2.position.Value, out collisionNormal);
        } 
        
        if (entity1.hasSphereCollider && entity2.hasBoxCollider) 
        {
            return AABBvsSphere(entity2.boxCollider, entity2.position.Value, entity1.sphereCollider, 
                entity1.position.Value, out collisionNormal);
        } 
        
        if (entity1.hasSphereCollider && entity2.hasSphereCollider) 
        {
            return SpherevsSphere(entity1.sphereCollider, entity1.position.Value, entity2.sphereCollider, 
                entity2.position.Value, out collisionNormal);
        }

        return false;
    }

    private static bool AABBvsSphere(BoxColliderComponent a, Vector3 aPos, SphereColliderComponent b, Vector3 bPos, out Vector3 collisionNormal) {
        Vector3 closestPoint = new Vector3(
            Mathf.Clamp(bPos.x, aPos.x - a.Size.x / 2, aPos.x + a.Size.x / 2),
            Mathf.Clamp(bPos.y, aPos.y - a.Size.y / 2, aPos.y + a.Size.y / 2),
            Mathf.Clamp(bPos.z, aPos.z - a.Size.z / 2, aPos.z + a.Size.z / 2)
        );

        Vector3 direction = bPos - closestPoint;
        float distanceSquared = direction.sqrMagnitude;
        collisionNormal = (closestPoint - aPos).normalized;

        return distanceSquared < (b.Radius * b.Radius);
    }

    
    private static bool SpherevsSphere(SphereColliderComponent a, Vector3 aPos, SphereColliderComponent b, Vector3 bPos, out Vector3 collisionNormal) {
        Vector3 direction = aPos - bPos;
        float distanceSquared = direction.sqrMagnitude;
        collisionNormal = direction.normalized;

        return distanceSquared < ((a.Radius + b.Radius) * (a.Radius + b.Radius));
    }


    private static bool AABBvsAABB(BoxColliderComponent a, Vector3 aPos, BoxColliderComponent b, Vector3 bPos, out Vector3 collisionNormal) {
        collisionNormal = (aPos - bPos).normalized;
        
        if (Mathf.Abs(aPos.x - bPos.x) > (a.Size.x / 2 + b.Size.x / 2)) return false;
        if (Mathf.Abs(aPos.y - bPos.y) > (a.Size.y / 2 + b.Size.y / 2)) return false;
        if (Mathf.Abs(aPos.z - bPos.z) > (a.Size.z / 2 + b.Size.z / 2)) return false;

        return true;
    }
}