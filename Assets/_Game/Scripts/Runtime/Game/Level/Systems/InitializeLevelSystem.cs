using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class InitializeLevelSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private readonly Contexts _contexts;
    private Transform _playAreaContainer;
    private readonly ILevelService _levelService;

    public InitializeLevelSystem(Contexts contexts, Services services) : base(contexts.game)
    {
        _contexts = contexts;
        _levelService = services.LevelService;
    }

    public void Initialize()
    {
        _levelService.RefreshData();
        SetupLevel();
    }

    private void SetupLevel()
    {
        if (_levelService.CurrentLevel >= _levelService.TotalLevelCount)
        {
            _levelService.CurrentLevel = Random.Range(0, _levelService.TotalLevelCount);
        }
        
        _contexts.game.ReplaceCreatedObjectsCount(0);
        _contexts.game.ReplaceRemainingObjectsCount(0);
        _contexts.input.isInputBlock = true;

        _levelService.CreatedObjectCount = 0;
        
        _contexts.game.isLevelReady = true;
        _contexts.input.isInputBlock = false;
        
        _levelService.LevelStatus = LevelStatus.Continue;
        _contexts.game.ReplaceLevelStatus(_levelService.LevelStatus);
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
        
        Debug.Log("Level Loaded!");

        _contexts.game.isLevelEnd = false;
        TimeService.Instance.ResumeTime();
        SetupLevel();
    }
}
