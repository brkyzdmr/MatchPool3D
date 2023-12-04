public sealed class LevelFeature : Feature
{
    public LevelFeature(Contexts contexts, Services services)
    {
        Add(new InitializeLevelSystem(contexts, services));
        Add(new WinCheckSystem(contexts, services));
        Add(new FailCheckSystem(contexts, services));
    }
}
