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
            var randomPos = new Vector3(Random.Range(0, 10), 10, Random.Range(0, 10));
            _services.BallProduceService.CreateBall(randomPos);
        }
    }
}