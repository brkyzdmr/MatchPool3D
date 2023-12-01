public sealed class LevelFeature : Feature
{
    public LevelFeature(Contexts contexts)
    {
        Add(new InitializeLevelSystem(contexts));
        Add(new WinCheckSystem(contexts));
        Add(new FailCheckSystem(contexts));
    }
}
