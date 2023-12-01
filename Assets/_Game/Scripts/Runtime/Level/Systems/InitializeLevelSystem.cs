using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class InitializeLevelSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private readonly Contexts _contexts;
    private Transform _playAreaContainer;
    private int _levelCount;
    
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
        if (LevelService.PlayerCurrentLevel >= _levelCount)
        {
            LevelService.PlayerCurrentLevel = Random.Range(0, _levelCount);
        }
        
        var level = LevelService.PlayerCurrentLevel;
        var config = _contexts.config.levelConfig.value;
        _contexts.game.ReplaceCreatedObjectsCount(0);
        _contexts.game.ReplaceRemainingObjectsCount(config.Levels.levels[level].maxProducedObjectLevel);
        _contexts.input.isInputBlock = true;
        
        CreateObjects();
        _contexts.game.isLevelReady = true;
        _contexts.input.isInputBlock = false;
        
        LevelService.LevelStatus = LevelStatus.Continue;
        _contexts.game.ReplaceLevelStatus(LevelService.LevelStatus);
    }

    private void CreateObjects()
    {
        var cubeCount = 0;

        for (int i = 0; i < 10; i++)
        {
            var randomPosition = new Vector3(Random.Range(-5, 5), 10, Random.Range(-5, 5));
            _contexts.game.CreateObject(randomPosition);
        }
        
        _contexts.game.ReplaceCreatedObjectsCount(cubeCount);
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
        SetupLevel();
    }
}
