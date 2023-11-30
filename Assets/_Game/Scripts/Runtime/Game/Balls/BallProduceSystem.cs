using Entitas;
using UnityEngine;

public class BallProduceSystem : IInitializeSystem
{
    private readonly Contexts _contexts;
    private readonly Services _services;

    public BallProduceSystem(Contexts contexts, Services services)
    {
        _contexts = contexts;
        _services = services;
    }
    
    public void Initialize()
    {
        var id = _services.IdService.GetNext();
        var pool = _services.ObjectProduceService.CreatePool(id, Vector3.zero);
        
        for (int i = 0; i < 50; i++)
        {
            var randomPos = new Vector3(Random.Range(-5, 5), 20, Random.Range(-5, 5));
            id = _services.IdService.GetNext();
            var entity = _services.ObjectProduceService.CreateBall(id, randomPos);
        }
    }
}
