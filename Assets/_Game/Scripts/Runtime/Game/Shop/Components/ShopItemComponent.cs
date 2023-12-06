using Entitas;
using UnityEngine;

[Game]
public class ShopItemComponent : IComponent {
    public string Type;
    public Sprite Icon;
    public string Name;
    public int Price;
    public bool IsPurchased;
}