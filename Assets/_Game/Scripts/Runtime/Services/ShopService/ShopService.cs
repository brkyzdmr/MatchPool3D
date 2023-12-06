
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopService : Service, IShopService
{
    private Contexts _contexts;

    public ShopService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
    }
}
