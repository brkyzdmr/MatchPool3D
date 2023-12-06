
using System.Linq;
using Entitas;

public class InitializeShopSystem: IInitializeSystem 
{
    private readonly Contexts _contexts;
    private readonly IObjectService _objectService;

    public InitializeShopSystem(Contexts contexts) 
    {
        _contexts = contexts;
        _objectService = Services.GetService<IObjectService>();
    }

    public void Initialize()
    {
        var allItems = _contexts.config.objectsConfig.value.Config.objects;
        var availableItems = _objectService.GetAllAvailableObjects();

        foreach (var item in allItems) 
        {
            var isAvailable = availableItems.Any(availableItem => availableItem.type == item.type);

            _contexts.game.CreateEntity().AddShopItem(
                item.type, 
                null, // no icon yet
                item.shop.name, 
                item.shop.price, 
                isAvailable
            );
        }
    }
}
