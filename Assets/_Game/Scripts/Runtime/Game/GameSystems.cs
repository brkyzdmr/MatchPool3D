public class GameSystems : Feature
{
    public GameSystems(Contexts contexts, Services services)
    {
        Add(new BallProduceSystem(contexts, services));
        Add(new ViewSystem(contexts, services));
        
        Add(new PhysicSystems(contexts, services));
    }
}