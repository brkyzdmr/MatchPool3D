using UnityEngine;

public static class LevelService
{
    public static LevelStatus LevelStatus;
    public static int PlayerCurrentLevel
    {
        get => PlayerPrefs.GetInt(SaveGameService.CurrentLevelKey, 0);
        set => PlayerPrefs.SetInt(SaveGameService.CurrentLevelKey, value);
    }

    public static readonly ILevelsConfig LevelsConfig = Contexts.sharedInstance.config.levelConfig.value;

    private static readonly Contexts Contexts = Contexts.sharedInstance;
    public static bool IsLevelCompleted()
    {
        var createdObjectsCount = Contexts.game.createdObjectsCount.Value;
        var remainingObjectsCount = Contexts.game.remainingObjectsCount.Value;
        var levelObjectsCount = LevelsConfig.Levels.levels[PlayerCurrentLevel].maxProducedObjectCount;

        var isComplete = (createdObjectsCount == levelObjectsCount) && (remainingObjectsCount == 0);

        return isComplete;
    }

    public static void SetLevelStatus(LevelStatus status)
    {
        Contexts.game.ReplaceLevelStatus(status);
        Debug.Log("Level Status Changed: " + status);
    }
}