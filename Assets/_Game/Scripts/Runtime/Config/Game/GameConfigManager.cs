using UnityEngine;

public static class GameConfigManager
{
    private const string ConfigFilePath = "Data/GameConfig";

    public static IGameConfig LoadGameConfig()
    {
        var data = JsonConfigReader.ReadJsonConfig<GameConfigData>(ConfigFilePath);

        return new GameConfigImplementation(data);
    }

    private class GameConfigImplementation : IGameConfig
    {
        public GameConfigData GameConfig { get; }

        public GameConfigImplementation(GameConfigData config)
        {
            GameConfig = config;
        }
    }
}