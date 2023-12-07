using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class GenerateObjectsSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private Transform _playAreaContainer;
    private readonly ILevelService _levelService;
    private readonly ITimeService _timeService;
    private readonly IObjectService _objectService;
    private readonly IObjectProductionService _objectProductionService;

    public GenerateObjectsSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _levelService = Services.GetService<ILevelService>();
        _objectService = Services.GetService<IObjectService>();
        _objectProductionService = Services.GetService<IObjectProductionService>();
    }

    private void SetupObjects()
    {
        var availableObjects = _objectService.GetAllAvailableObjects();
        var objectList = _objectProductionService.GenerateObjects(availableObjects);
        
        _contexts.game.ReplaceGeneratedObjects(objectList);
    }
    
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.LevelReady);

    protected override bool Filter(GameEntity entity) => _contexts.game.isLevelReady;
    
    protected override void Execute(List<GameEntity> entities)
    {
        _contexts.game.ReplaceDebugLog("Objects list refreshed!");   
        SetupObjects();
    }
}