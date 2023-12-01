public static class LevelsConfigManager
{
    private const string ConfigFilePath = "Data/Levels";

    public static ILevelsConfig LoadGameConfig()
    {
        var data = JsonConfigReader.ReadJsonConfig<LevelsConfigData>(ConfigFilePath);

        return new LevelsesConfigImplementation(data);
    }

    private class LevelsesConfigImplementation : ILevelsConfig
    {
        public LevelsConfigData Config { get; }

        public LevelsesConfigImplementation(LevelsConfigData config)
        {
            Config = config;
        }
    }
}