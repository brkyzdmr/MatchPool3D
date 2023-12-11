using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class LevelLoadSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly ILevelService _levelService;
    private readonly ITimeService _timeService;
    private readonly ISaveService _saveService;

    public LevelLoadSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _levelService = Services.GetService<ILevelService>();
        _timeService = Services.GetService<ITimeService>();
        _saveService = Services.GetService<ISaveService>();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.LoadLevel);

    protected override bool Filter(GameEntity entity) => _contexts.game.isLoadLevel;
    
    protected override void Execute(List<GameEntity> entities)
    {
        var mergableObjects = _contexts.game.GetEntities(GameMatcher.MergableObject);
        foreach (var mergableObject in mergableObjects)
        {
            mergableObject.isDestroyed = true;
        }
        
        Debug.Log("LevelLoadSystem: " + _contexts.game.isLevelRestart);
        
        _contexts.game.isLevelReady = false;
        _contexts.game.isLevelEnd = false;
        _contexts.game.isLevelRestart = false;
        _contexts.game.isLoadLevel = false;
        _timeService.ResumeTime();
        _levelService.SetupLevel();
    }
}