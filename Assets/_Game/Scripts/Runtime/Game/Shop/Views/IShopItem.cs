using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IShopItem
{
    public string Type { get; set; }
    public Image Icon { get; set; }
    public TMP_Text ShopName { get; set; }
    public TMP_Text ShopPrice { get; set; }
    public Button BuyButton { get; set; }

    void SetItem(string type, Sprite icon, string name, string price);
    void SetAction(UnityAction action);
    public void ItemBought();
}
