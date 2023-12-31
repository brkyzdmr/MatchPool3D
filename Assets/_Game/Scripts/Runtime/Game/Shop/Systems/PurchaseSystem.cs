﻿using System.Collections.Generic;
using Entitas;
using MoreMountains.NiceVibrations;

public class PurchaseSystem : ReactiveSystem<GameEntity> 
{
    private readonly Contexts _contexts;
    private readonly ILevelService _levelService;
    private readonly IObjectService _objectService;
    private readonly IVibrationService _vibrationService;
    private readonly ISaveService _saveService;

    public PurchaseSystem(Contexts contexts) : base(contexts.game) 
    {
        _contexts = contexts;
        _objectService = Services.GetService<IObjectService>();
        _levelService = Services.GetService<ILevelService>();
        _vibrationService = Services.GetService<IVibrationService>();
        _saveService = Services.GetService<ISaveService>();
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) => 
        context.CreateCollector(GameMatcher.PurchaseAction);

    protected override bool Filter(GameEntity entity) => entity.hasPurchaseAction;

    protected override void Execute(List<GameEntity> entities) 
    {
        foreach (var e in entities)
        {
            var itemType = e.purchaseAction.ItemType;
            var totalGold = _contexts.game.totalGold.Value;
            var price = e.purchaseAction.Price;

           var isObjectBought = _objectService.IsObjectInAvailableObjects(itemType);
            
            if (totalGold >= price && !isObjectBought)
            {
                _contexts.game.ReplaceTotalGold(_contexts.game.totalGold.Value - price);
                _contexts.game.isGoldEarned = true;
                _objectService.SetAvailableObjectByType(itemType, true);
                _contexts.game.ReplaceItemPurchased(itemType);
                _vibrationService.PlayHaptic(HapticTypes.MediumImpact);
                _saveService.Save();
            }
            
            e.isDestroyed = true; 
        }
    }
}