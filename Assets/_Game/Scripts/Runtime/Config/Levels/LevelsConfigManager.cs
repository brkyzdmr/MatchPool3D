public static class LevelsConfigManager
{
    private const string ConfigFilePath = "Data/Levels";

    public static ILevelsConfig LoadLevelsConfig()
    {
        var data = JsonConfigReader.ReadJsonConfig<LevelsConfigData>(ConfigFilePath);

        return new LevelsConfigImplementation(data);
    }

    private class LevelsConfigImplementation : ILevelsConfig
    {
        public LevelsConfigData Levels { get; }

        public LevelsConfigImplementation(LevelsConfigData config)
        {
            Levels = config;
        }
    }
}