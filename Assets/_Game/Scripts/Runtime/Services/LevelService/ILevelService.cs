public interface ILevelService
{
    ILevelsConfig LevelsConfig { get; }
    LevelStatus LevelStatus { get; set; }
    int CurrentLevel { get; set; }
    int TotalGold { get; set; }
    int AvailableObjects { get; set; }
    int MaxProducedObjectCount { get; }
    int MaxProducedObjectLevel { get; }
    int MaxObjectLevel { get; }
    int TotalLevelCount { get; }
    int CreatedObjectCount { get; set; }
    
    bool IsLevelCompleted();
    void SetLevelStatus(LevelStatus status);
    void RefreshData();
}