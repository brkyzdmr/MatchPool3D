using UnityEngine;

public static class LevelService
{
    public static LevelStatus LevelStatus;
    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt(SaveGameService.CurrentLevelKey, 0);
        set => PlayerPrefs.SetInt(SaveGameService.CurrentLevelKey, value);
    }

    public static int TotalGold
    {
        get => PlayerPrefs.GetInt(SaveGameService.TotalGoldKey, 0);
        set => PlayerPrefs.SetInt(SaveGameService.TotalGoldKey, value);
    }
    
    public static int AvailableObjects
    {
        get => PlayerPrefs.GetInt(SaveGameService.AvailableObjectsKey, 1);
        set => PlayerPrefs.SetInt(SaveGameService.AvailableObjectsKey, value);
    }

    public static readonly ILevelsConfig LevelsConfig = Contexts.sharedInstance.config.levelConfig.value;
    public static readonly int MaxProducedObjectCount = LevelsConfig.Levels.levels[CurrentLevel].maxProducedObjectCount;
    public static readonly int MaxProducedObjectLevel = LevelsConfig.Levels.levels[CurrentLevel].maxProducedObjectLevel;
    public static readonly int MaxObjectLevel = LevelsConfig.Levels.levels[CurrentLevel].maxObjectLevel;
    public static readonly int TotalLevelCount = LevelsConfig.Levels.levels.Count;
    public static int CreatedObjectCount = 0;

    private static readonly Contexts Contexts = Contexts.sharedInstance;
    public static bool IsLevelCompleted()
    {
        var createdObjectsCount = Contexts.game.createdObjectsCount.Value;
        var remainingObjectsCount = Contexts.game.remainingObjectsCount.Value;
        var levelObjectsCount = LevelsConfig.Levels.levels[CurrentLevel].maxProducedObjectCount;

        var isComplete = (createdObjectsCount == levelObjectsCount) && (remainingObjectsCount == 0);

        return isComplete;
    }

    public static void SetLevelStatus(LevelStatus status)
    {
        Contexts.game.ReplaceLevelStatus(status);
    }
}