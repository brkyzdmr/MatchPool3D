
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game, Unique, Event(EventTarget.Any)]
public class PurchaseActionComponent : IComponent
{
    public string ItemType;
    public int Price;
}
