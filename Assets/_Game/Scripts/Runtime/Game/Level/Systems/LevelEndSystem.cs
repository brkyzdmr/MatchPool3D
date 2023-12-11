
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class LevelEndSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly ISaveService _saveService;

    public LevelEndSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _saveService = Services.GetService<ISaveService>();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.LevelEnd);

    protected override bool Filter(GameEntity entity) => _contexts.game.isLevelEnd;
    
    protected override void Execute(List<GameEntity> entities)
    {
        var mergableObjects = _contexts.game.GetEntities(GameMatcher.MergableObject);
        foreach (var mergableObject in mergableObjects)
        {
            mergableObject.isDestroyed = true;
        }
        
        Debug.Log("LevelEndSystem: " + _contexts.game.isLevelEnd);
    }
}
