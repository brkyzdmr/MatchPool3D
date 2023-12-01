public static class ObjectsConfigManager
{
    private const string ConfigFilePath = "Data/GameConfig";

    public static IObjectsConfig LoadGameConfig()
    {
        var data = JsonConfigReader.ReadJsonConfig<ObjectsConfigData>(ConfigFilePath);

        return new ObjectsConfigImplementation(data);
    }

    private class ObjectsConfigImplementation : IObjectsConfig
    {
        public ObjectsConfigData Config { get; }

        public ObjectsConfigImplementation(ObjectsConfigData config)
        {
            Config = config;
        }
    }
}
