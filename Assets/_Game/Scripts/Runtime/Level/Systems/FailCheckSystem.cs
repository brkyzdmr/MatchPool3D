
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class FailCheckSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    
    public FailCheckSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.LevelEnd);

    protected override bool Filter(GameEntity entity) => _contexts.game.isLevelEnd; 
    
    protected override void Execute(List<GameEntity> entities)
    {
        if (!LevelService.IsLevelCompleted())
        {
            LevelService.SetLevelStatus(LevelStatus.Fail);
        }
    }
}
