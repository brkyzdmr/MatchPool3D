public sealed class GameSystems : Feature
{
    public GameSystems(Contexts contexts)
    {
        // Game Features
        Add(new TimeFeature(contexts));
        Add(new LevelFeature(contexts));
        Add(new InputFeature(contexts));
        Add(new ViewFeature(contexts));
        Add(new ObjectFeature(contexts));

        // (Generated)
        Add(new GameEventSystems(contexts));
        // Add(new GameCleanupSystems(contexts));
    }
}
