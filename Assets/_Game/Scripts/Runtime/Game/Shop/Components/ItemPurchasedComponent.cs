using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique, Event(EventTarget.Any)]
public class ItemPurchasedComponent : IComponent
{
    public string ItemType;
}
