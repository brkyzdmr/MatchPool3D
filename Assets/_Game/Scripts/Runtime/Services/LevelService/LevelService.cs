using UnityEngine;

public class LevelService : Service, ILevelService
{
    private readonly Contexts _contexts;

    public LevelService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
    }

    private ISaveService SaveService => Services.GetService<ISaveService>();

    public ILevelsConfig LevelsConfig
    {
        get => _contexts.config.levelsConfig.value;
    }

    public LevelStatus LevelStatus
    {
        get => _contexts.game.levelStatus.Value;
        set => _contexts.game.ReplaceLevelStatus(value);
    }

    public int CurrentLevel
    {
        get => SaveService.GetInt(SaveService.CurrentLevelKey, 0);
        set
        {
            SaveService.SetInt(SaveService.CurrentLevelKey, value);
            RefreshData();
        }
    }

    public int TotalGold
    {
        get => SaveService.GetInt(SaveService.TotalGoldKey, 0);
        set => SaveService.SetInt(SaveService.TotalGoldKey, value);
    }

    public int AvailableObjects
    {
        get => SaveService.GetInt(SaveService.AvailableObjectsKey, 1);
        set => SaveService.SetInt(SaveService.AvailableObjectsKey, value);
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
        var levelObjectsCount = LevelsConfig.Levels.levels[CurrentLevel].maxProducedObjectCount;

        return (createdObjectsCount == levelObjectsCount) && (remainingObjectsCount == 0);
    }

    public void SetLevelStatus(LevelStatus status)
    {
        Contexts.game.ReplaceLevelStatus(status);
    }

    public void RefreshData()
    {
        MaxProducedObjectCount = LevelsConfig.Levels.levels[CurrentLevel].maxProducedObjectCount;
        MaxProducedObjectLevel = LevelsConfig.Levels.levels[CurrentLevel].maxProducedObjectLevel;
        MaxObjectLevel = LevelsConfig.Levels.levels[CurrentLevel].maxObjectLevel;
        TotalLevelCount = LevelsConfig.Levels.levels.Count;
    }
}
