using Entitas;
using UnityEngine;

namespace _Game.Scripts.Runtime.Balls
{
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
            for (int i = 0; i < 50; i++)
            {
                var randomPos = new Vector3(Random.Range(-5, 5), 20, Random.Range(-5, 5));
                var id = _services.IdService.GetNext();
                var entity = _services.BallProduceService.CreateBall(id, randomPos);
            }
        }
    }
}