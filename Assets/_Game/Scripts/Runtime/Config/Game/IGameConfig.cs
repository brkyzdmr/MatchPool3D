using Entitas.CodeGeneration.Attributes;

[Config, Unique, ComponentName("GameConfig")]
public interface IGameConfig
{
    public GameConfigData GameConfig { get; }
}
