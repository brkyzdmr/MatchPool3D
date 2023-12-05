﻿using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ObjectMergeSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly float _mergeVelocityThreshold = 1f;
    private ILevelService _levelService;
    private IObjectService _objectService;

    public ObjectMergeSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;

        _levelService = Services.GetService<ILevelService>();
        _objectService = Services.GetService<IObjectService>();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(
            Matcher<GameEntity>.AllOf(
                GameMatcher.Collision,
                GameMatcher.Object
            )
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCollision && entity.hasObject && entity.isMergableObject;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            if (!entity.hasCollision || entity.isDestroyed) { continue; }

            var collisionData = entity.collision;

            if (collisionData.OtherEntity != null &&
                collisionData.OtherEntity.isEnabled && 
                collisionData.RelativeVelocity.magnitude > _mergeVelocityThreshold)
            {
                var thisObject = entity.@object;
                var otherObject = collisionData.OtherEntity.@object;

                if (thisObject.Type == otherObject.Type && thisObject.Level == otherObject.Level)
                {
                    MergeObjects(entity, collisionData.OtherEntity);
                }
            }
        }
    }

    private void MergeObjects(GameEntity entity1, GameEntity entity2)
    {
        var maxLevel = _contexts.game.maxObjectLevel.Value;
        var nextLevel = CalculateNextLevel(entity1.@object.Level);
        
        if (nextLevel <= maxLevel)
        {
            var path = _objectService.GetObjectPath(entity1.@object.Type, nextLevel);
            var mergedEntity = _contexts.game.CreateObject(entity1.@object.Type, nextLevel, path,
                entity1.collision.CollisionPoint);
            _contexts.game.ReplaceRemainingObjectsCount(_contexts.game.remainingObjectsCount.Value + 1);
        }
        
        entity1.isDestroyed = true;
        entity2.isDestroyed = true;
        _contexts.game.ReplaceRemainingObjectsCount(_contexts.game.remainingObjectsCount.Value - 2);
    }

    private int CalculateNextLevel(int currentLevel)
    {
        return currentLevel + 1;
    }
}
