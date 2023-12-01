using System.Collections.Generic;
using Entitas.CodeGeneration.Attributes;

[Config, Unique, ComponentName("LevelConfig")]
public interface ILevelsConfig
{
    public List<LevelsConfigData> Levels => new List<LevelsConfigData>();
}