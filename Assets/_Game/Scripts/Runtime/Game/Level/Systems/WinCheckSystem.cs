﻿
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class WinCheckSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly ILevelService _levelService;
    private readonly ITimeService _timeService;

    public WinCheckSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _levelService = Services.GetService<ILevelService>();
        _timeService = Services.GetService<ITimeService>();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.RemainingObjectsCount);

    protected override bool Filter(GameEntity entity) => _contexts.game.hasRemainingObjectsCount;

    protected override void Execute(List<GameEntity> entities)
    {
        if (_levelService.IsLevelCompleted())
        {
            _levelService.SetLevelStatus(LevelStatus.Win);
            _contexts.game.isLevelReady = false;
            _contexts.game.isLevelEnd = true;
            _timeService.PauseTime();
        }
    }
}