
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class WinCheckSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;

    public WinCheckSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.RemainingObjectsCount);

    protected override bool Filter(GameEntity entity) => _contexts.game.hasRemainingObjectsCount;

    protected override void Execute(List<GameEntity> entities)
    {
        if (LevelService.IsLevelCompleted())
        {
            LevelService.SetLevelStatus(LevelStatus.Win);
            _contexts.game.isLevelReady = false;
            _contexts.game.isLevelEnd = true;
            TimeService.Instance.PauseTime();
        }
    }
}