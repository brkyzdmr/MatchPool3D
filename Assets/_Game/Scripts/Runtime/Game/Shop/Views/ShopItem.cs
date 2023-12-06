using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IShopItem
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text shopName;
    [SerializeField] private TMP_Text shopPrice;
    [SerializeField] private Button buyButton;

    public string Type { get; set; }

    public Image Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    public TMP_Text ShopName
    {
        get { return shopName; }
        set { shopName = value; }
    }

    public TMP_Text ShopPrice
    {
        get { return shopPrice; }
        set { shopPrice = value; }
    }

    public Button BuyButton
    {
        get { return buyButton; }
        set { buyButton = value; }
    }

    public void SetItem(string type, Sprite icon, string name, string price)
    {
        Type = type;
        Icon.sprite = icon;
        ShopName.text = name;
        ShopPrice.text = price;
    }

    public void SetAction(UnityAction action)
    {
        if (buyButton != null)
        {
            BuyButton.onClick.AddListener(action);
        }
    }

    public void ItemBought()
    {
        buyButton.interactable = false;
        buyButton.GetComponentInChildren<TMP_Text>().text = "Bought";
    }
}
