using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class SaveSystem : ReactiveSystem<GameEntity> 
{
    private readonly Contexts _contexts;
    private readonly ISaveService _saveService;

    public SaveSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _saveService = Services.GetService<ISaveService>();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.Save);

    protected override bool Filter(GameEntity entity) => _contexts.game.isSave; 

    protected override void Execute(List<GameEntity> entities)
    {
        _contexts.game.ReplaceDebugLog("Save!");
        
        _saveService.SetInt(_saveService.CurrentLevelKey, _contexts.game.currentLevelIndex.Value);
        _saveService.SetInt(_saveService.TotalGoldKey, _contexts.game.totalGold.Value);
        _saveService.SetInt(_saveService.AvailableObjectsKey, _contexts.game.availableObjects.Value);
        _contexts.game.isSave = false;
    }
}