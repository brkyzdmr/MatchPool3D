
public class GameService : Service, IGameService
{
    private Contexts _contexts;
    
    public GameService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
    }
    
    public IGameConfig GameConfig => _contexts.config.gameConfig.value;
}
