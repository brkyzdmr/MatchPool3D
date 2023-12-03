using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class InitializeLevelSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private readonly Contexts _contexts;
    private Transform _playAreaContainer;
    
    public InitializeLevelSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        SetupLevel();
    }

    private void SetupLevel()
    {
        if (LevelService.CurrentLevel >= LevelService.TotalLevelCount)
        {
            LevelService.CurrentLevel = Random.Range(0, LevelService.TotalLevelCount);
        }
        
        _contexts.game.ReplaceCreatedObjectsCount(0);
        _contexts.game.ReplaceRemainingObjectsCount(0);
        _contexts.input.isInputBlock = true;

        LevelService.CreatedObjectCount = 0;
        
        _contexts.game.isLevelReady = true;
        _contexts.input.isInputBlock = false;
        
        LevelService.LevelStatus = LevelStatus.Continue;
        _contexts.game.ReplaceLevelStatus(LevelService.LevelStatus);
    }
    
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.LoadLevel);
    
    protected override bool Filter(GameEntity entity) => _contexts.game.isLevelEnd;
    
    protected override void Execute(List<GameEntity> entities)
    {
        var mergableObjects = _contexts.game.GetEntities(GameMatcher.MergableObject);
        foreach (var mergableObject in mergableObjects)
        {
            mergableObject.isDestroyed = true;
        }

        _contexts.game.isLevelEnd = false;
        TimeService.Instance.ResumeTime();
        SetupLevel();
    }
}
