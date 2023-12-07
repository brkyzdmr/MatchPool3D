
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class DebugLogMessageSystem : ReactiveSystem<GameEntity> 
{
    private readonly Contexts _contexts;

    public DebugLogMessageSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(
            Matcher<GameEntity>.AllOf(GameMatcher.DebugLog)
        );

    protected override bool Filter(GameEntity entity) => entity.hasDebugLog;

    protected override void Execute (List<GameEntity> entities) {
        foreach (var e in entities) {
            Debug.Log(e.debugLog.Message);
            e.isDestroyed = true;
        }
    }
}
