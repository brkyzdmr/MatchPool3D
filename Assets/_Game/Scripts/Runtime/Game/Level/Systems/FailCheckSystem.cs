﻿
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class FailCheckSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly ILevelService _levelService;

    public FailCheckSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _levelService = Services.GetService<ILevelService>();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.LevelEnd);

    protected override bool Filter(GameEntity entity) => _contexts.game.isLevelEnd; 
    
    protected override void Execute(List<GameEntity> entities)
    {
        if (!_levelService.IsLevelCompleted())
        {
            _levelService.SetLevelStatus(LevelStatus.Fail);
            _contexts.game.isLevelReady = false;
            _contexts.game.isLevelEnd = true;
            TimeService.Instance.PauseTime();
        }
    }
}
