using System;
using Entitas;

public class GameController
{
    readonly Systems _systems;
    readonly Contexts _contexts;

    public GameController(Contexts contexts/*, IGameConfig gameConfig*/)
    {
        _contexts = contexts;
        var random = new Random(DateTime.UtcNow.Millisecond);
        UnityEngine.Random.InitState(random.Next());
        Rand.game = new Rand(random.Next());

        _systems = new GameSystems(contexts);
    }

    public void ReplaceObjectsConfig(IObjectsConfig objectsConfig)
    {
        _contexts.config.ReplaceObjectsConfig(objectsConfig);   
    }

    public void ReplaceLevelsConfig(ILevelsConfig levelsConfig)
    {
        _contexts.config.ReplaceLevelConfig(levelsConfig);
    }

    public void ReplaceGameConfig(IGameConfig gameConfig)
    {
        _contexts.config.ReplaceGameConfig(gameConfig);
    }

    public void Initialize()
    {
        // This calls Initialize() on all sub systems
        _systems.Initialize();
    }

    public void Execute()
    {
        // This calls Execute() and Cleanup() on all sub systems
        _systems.Execute();
        _systems.Cleanup();
    }
}
