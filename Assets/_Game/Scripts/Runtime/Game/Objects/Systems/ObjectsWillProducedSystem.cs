using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class ObjectsWillProducedSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private Transform _playAreaContainer;
    private readonly ILevelService _levelService;
    private readonly ITimeService _timeService;
    private readonly IObjectService _objectService;

    public ObjectsWillProducedSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _levelService = Services.GetService<ILevelService>();
        _objectService = Services.GetService<IObjectService>();
    }

    private void SetupObjects()
    {
        var availableObjects = _objectService.GetAllAvailableObjects();
        var objectList = _objectService.CalculateObjectsWillProduced(availableObjects);
        
        _contexts.game.ReplaceObjectsWillProduced(objectList);
    }
    
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.LevelReady);

    protected override bool Filter(GameEntity entity) => _contexts.game.isLevelReady;
    
    protected override void Execute(List<GameEntity> entities)
    {
        Debug.Log("Objects list refreshed!");

        SetupObjects();
    }
}