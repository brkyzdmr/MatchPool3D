using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class LoadSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private readonly Contexts _contexts;
    private readonly ISaveService _saveService;
    private readonly ILevelService _levelService;

    public LoadSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _saveService = Services.GetService<ISaveService>();
        _levelService = Services.GetService<ILevelService>();
    }

    public void Initialize()
    {
        LoadData();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.Load);

    protected override bool Filter(GameEntity entity) => _contexts.game.isLoad;

    protected override void Execute(List<GameEntity> entities)
    {
        LoadData();
    }

    private void LoadData()
    {
        _contexts.game.ReplaceDebugLog("Load!");

        _contexts.game.ReplaceCurrentLevelIndex(_saveService.GetInt(_saveService.CurrentLevelKey, 0));
        _contexts.game.ReplaceTotalGold(_saveService.GetInt(_saveService.TotalGoldKey, 0));
        _contexts.game.ReplaceAvailableObjects(_saveService.GetInt(_saveService.AvailableObjectsKey, 1));
        _contexts.game.isLoad = false;
    }
}