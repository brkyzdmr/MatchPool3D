using Entitas.CodeGeneration.Attributes;

[Config, Unique, ComponentName("LevelConfig")]
public interface ILevelsConfig
{
    public LevelsConfigData Levels { get; }
}