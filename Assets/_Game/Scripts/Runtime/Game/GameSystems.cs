using _Game.Scripts.Runtime.Balls;

public class GameSystems : Feature
{
    public GameSystems(Contexts contexts, Services services)
    {
        Add(new BallProduceSystem(contexts, services));
        
        Add(new ViewSystem(contexts, services));
    }
}