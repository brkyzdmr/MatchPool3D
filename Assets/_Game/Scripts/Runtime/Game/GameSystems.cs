public sealed class GameSystems : Feature
{
    public GameSystems(Contexts contexts, Services services)
    {
        // Game Features
        Add(new TimeFeature(contexts));
        Add(new LevelFeature(contexts, services));
        Add(new InputFeature(contexts, services));
        Add(new ViewFeature(contexts));
        Add(new ObjectFeature(contexts, services));

        // (Generated)
        Add(new GameEventSystems(contexts));
        // Add(new GameCleanupSystems(contexts));
    }
}
