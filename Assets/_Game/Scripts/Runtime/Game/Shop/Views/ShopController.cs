using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Entitas;

public class ShopController : MonoBehaviour, IAnyItemPurchasedListener
{
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private Transform contentHolder;
    

    private Contexts _contexts;
    private IObjectService _objectService;
    private GameEntity _listener;
    private Dictionary<string, IShopItem> _shopItems = new Dictionary<string, IShopItem>();

    private void Start()
    {
        _contexts = Contexts.sharedInstance;
        _objectService = Services.GetService<IObjectService>();
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyItemPurchasedListener(this);

        InitializeShopItems();
    }

    private void InitializeShopItems()
    {
        var shopItemsGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.ShopItem));
        if (shopItemsGroup != null)
        {
            var shopItems = shopItemsGroup.GetEntities();
            foreach (var shopItem in shopItems)
            {
                if (shopItem != null && shopItem.hasShopItem)
                {
                    CreateShopItemUI(shopItem);
                }
            }
        }
    }

    private void CreateShopItemUI(GameEntity shopItemEntity)
    {
        var shopItemGO = Instantiate(shopItemPrefab, contentHolder);
        var shopItemUI = shopItemGO.GetComponent<IShopItem>();
        var shopItemIcon = _objectService.GetObjectSpriteByType(shopItemEntity.shopItem.Type);
        shopItemUI.SetItem(shopItemEntity.shopItem.Type, shopItemIcon, shopItemEntity.shopItem.Name,
            shopItemEntity.shopItem.Price.ToString());

        shopItemUI.SetAction(() => BuyItem(shopItemEntity.shopItem.Type, shopItemEntity.shopItem.Price));

        _shopItems[shopItemEntity.shopItem.Type] = shopItemUI;
        var isItemBought = _objectService.IsObjectInAvailableObjects(shopItemEntity.shopItem.Type);

        if (isItemBought)
        {
            shopItemUI.ItemBought();
        }
    }

    private void BuyItem(string itemType, int price)
    {
        _contexts.game.ReplacePurchaseAction(itemType, price);
    }

    public void OnAnyItemPurchased(GameEntity entity, string itemType)
    {
        _shopItems[itemType].ItemBought();
    }
}