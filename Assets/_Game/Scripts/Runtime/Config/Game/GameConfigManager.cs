public static class GameConfigManager
{
    private const string ConfigFilePath = "Data/Levels";

    public static IGameConfig LoadGameConfig()
    {
        var data = JsonConfigReader.ReadJsonConfig<GameConfigData>(ConfigFilePath);

        return new GameConfigImplementation(data);
    }

    private class GameConfigImplementation : IGameConfig
    {
        public GameConfigImplementation(GameConfigData config)
        {
        }
    }
}