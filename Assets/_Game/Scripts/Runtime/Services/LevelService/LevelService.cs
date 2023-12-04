using UnityEngine;

public class LevelService : Service, ILevelService
{
    private ILevelsConfig _levelsConfig;
    private readonly Contexts _contexts;
    private readonly Services _services;

    public LevelService(Contexts contexts, Services services) : base(contexts)
    {
        _contexts = contexts;
        _services = services;
    }

    public LevelStatus LevelStatus
    {
        get => _contexts.game.levelStatus.Value;
        set => _contexts.game.ReplaceLevelStatus(value);
    }

    public int CurrentLevel
    {
        get => _services.SaveService.GetInt(SaveService.CurrentLevelKey, 0);
        set
        {
            _services.SaveService.SetInt(SaveService.CurrentLevelKey, value);
            RefreshData();
        }
    }

    public int TotalGold
    {
        get => _services.SaveService.GetInt(SaveService.TotalGoldKey, 0);
        set => _services.SaveService.SetInt(SaveService.TotalGoldKey, value);
    }

    public int AvailableObjects
    {
        get => _services.SaveService.GetInt(SaveService.AvailableObjectsKey, 1);
        set => _services.SaveService.SetInt(SaveService.AvailableObjectsKey, value);
    }

    public int MaxProducedObjectCount { get; set; }
    public int MaxProducedObjectLevel { get; set; }
    public int MaxObjectLevel { get; set; }
    public int TotalLevelCount { get; set; }
    public int CreatedObjectCount { get; set; }
    

    public bool IsLevelCompleted()
    {
        var createdObjectsCount = _contexts.game.createdObjectsCount.Value;
        var remainingObjectsCount = _contexts.game.remainingObjectsCount.Value;
        var levelObjectsCount = _levelsConfig.Levels.levels[CurrentLevel].maxProducedObjectCount;

        return (createdObjectsCount == levelObjectsCount) && (remainingObjectsCount == 0);
    }

    public void SetLevelStatus(LevelStatus status)
    {
        Contexts.game.ReplaceLevelStatus(status);
    }

    public void RefreshData()
    {
        _levelsConfig = _contexts.config.levelsConfig.value;
        MaxProducedObjectCount = _levelsConfig.Levels.levels[CurrentLevel].maxProducedObjectCount;
        MaxProducedObjectLevel = _levelsConfig.Levels.levels[CurrentLevel].maxProducedObjectLevel;
        MaxObjectLevel = _levelsConfig.Levels.levels[CurrentLevel].maxObjectLevel;
        TotalLevelCount = _levelsConfig.Levels.levels.Count;
    }
}
