using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class LevelRestartSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly ILevelService _levelService;
    private readonly ITimeService _timeService;
    private readonly ISaveService _saveService;

    public LevelRestartSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _levelService = Services.GetService<ILevelService>();
        _timeService = Services.GetService<ITimeService>();
        _saveService = Services.GetService<ISaveService>();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.LevelRestart);

    protected override bool Filter(GameEntity entity) => _contexts.game.isLevelRestart;
    
    protected override void Execute(List<GameEntity> entities)
    {
        var mergableObjects = _contexts.game.GetEntities(GameMatcher.MergableObject);
        foreach (var mergableObject in mergableObjects)
        {
            mergableObject.isDestroyed = true;
        }
        
        Debug.Log("LevelRestartSystem: " + _contexts.game.isLevelRestart);
        
        _saveService.Load();
        
        _contexts.game.isLevelReady = false;
        _contexts.game.isLevelEnd = false;
        _contexts.game.isLevelRestart = false;
    }
}