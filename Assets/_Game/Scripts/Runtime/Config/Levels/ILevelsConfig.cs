using Entitas.CodeGeneration.Attributes;

[Config, Unique, ComponentName("LevelsConfig")]
public interface ILevelsConfig
{
    public LevelsConfigData Levels { get; }
}