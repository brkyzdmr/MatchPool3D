
public class ShopFeature : Feature
{
    public ShopFeature(Contexts contexts)
    {
        Add(new InitializeShopSystem(contexts));
        Add(new PurchaseSystem(contexts));
    }
}
