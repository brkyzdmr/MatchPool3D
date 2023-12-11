public sealed class LevelFeature : Feature
{
    public LevelFeature(Contexts contexts)
    {
        Add(new InitializeLevelSystem(contexts));
        Add(new LevelEndSystem(contexts));
        Add(new LevelRestartSystem(contexts));
        Add(new LevelLoadSystem(contexts));
        Add(new WinCheckSystem(contexts));
        Add(new FailCheckSystem(contexts));
    }
}
