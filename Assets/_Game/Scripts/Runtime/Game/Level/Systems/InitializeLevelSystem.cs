using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class InitializeLevelSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private readonly Contexts _contexts;
    private Transform _playAreaContainer;
    private readonly ILevelService _levelService;
    private readonly ITimeService _timeService;

    public InitializeLevelSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _levelService = Services.GetService<ILevelService>();
        _timeService = Services.GetService<ITimeService>();
    }

    public void Initialize()
    {
        SetupLevel();
    }

    private void SetupLevel()
    {
        _levelService.RefreshData();

        _contexts.game.ReplaceCreatedObjectsCount(0);
        _contexts.game.ReplaceRemainingObjectsCount(0);
        
        _contexts.input.isInputBlock = true;
        _contexts.game.createdObjectsCount.Value = 0;
        
        _contexts.game.isLevelReady = true;
        _contexts.input.isInputBlock = false;

        _contexts.game.ReplaceLevelStatus(LevelStatus.Continue);
    }
    
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.LoadLevel);

    protected override bool Filter(GameEntity entity) => _contexts.game.isLevelEnd || _contexts.game.isLevelRestart;
    
    protected override void Execute(List<GameEntity> entities)
    {
        var mergableObjects = _contexts.game.GetEntities(GameMatcher.MergableObject);
        foreach (var mergableObject in mergableObjects)
        {
            mergableObject.isDestroyed = true;
        }

        _contexts.game.isLevelReady = false;
        _contexts.game.isLevelEnd = false;
        _contexts.game.isLevelRestart = false;
        _timeService.ResumeTime();
        SetupLevel();
    }
}
