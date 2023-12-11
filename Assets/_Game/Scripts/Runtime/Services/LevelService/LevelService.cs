using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelService : Service, ILevelService
{
    private readonly Contexts _contexts;
    private readonly ITimeService _timeService;

    public LevelService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
        _timeService = Services.GetService<ITimeService>();
    }
    
    public ILevelsConfig LevelsConfig => _contexts.config.levelsConfig.value;

    public bool IsLevelCompleted()
    {
        return _contexts.game.createdObjectsCount.Value == _contexts.game.maxProducedObjectCount.Value
               && _contexts.game.remainingObjectsCount.Value == 0;
    }

    public void SetLevelStatus(LevelStatus status)
    {
        _contexts.game.ReplaceLevelStatus(status);
    }

    private void RefreshData()
    {
        if (_contexts.game.currentLevelIndex.Value < LevelsConfig.Levels.levels.Count)
        {
            LoadLevelData(_contexts.game.currentLevelIndex.Value);
        }
        else
        {
            LoadRandomLevel();
        }
    }
    
    public void SetupLevel()
    {
        RefreshData();

        _contexts.game.ReplaceCreatedObjectsCount(0);
        _contexts.game.ReplaceRemainingObjectsCount(0);
        
        _contexts.input.isInputBlock = true;
        _contexts.game.createdObjectsCount.Value = 0;
        
        _contexts.game.isLevelReady = true;
        _contexts.input.isInputBlock = false;

        _contexts.game.ReplaceLevelStatus(LevelStatus.Continue);
    }

    private void LoadLevelData(int levelIndex)
    {
        var levelData = LevelsConfig.Levels.levels[levelIndex];
        SetLevelData(levelData, levelData.name);
    }
    
    private void LoadRandomLevel()
    {
        var standardLevels = LevelsConfig.Levels.levels.Where(level => level.type == "standard").ToList();
        var randomLevel = standardLevels[Random.Range(0, standardLevels.Count)];
        // var actualIndexOfRandomLevel = LevelsConfig.Levels.levels.IndexOf(randomLevel);
        
        SetLevelData(randomLevel, GetRandomLevelName());
    }

    private void SetLevelData(LevelsConfigData.LevelData levelData, string levelName)
    {
        _contexts.game.ReplaceLevelName(levelName);
        _contexts.game.ReplaceLevelDuration(levelData.duration);
        _contexts.game.ReplaceMaxProducedObjectCount(levelData.maxProducedObjectCount);
        _contexts.game.ReplaceMaxProducedObjectLevel(levelData.maxProducedObjectLevel);
        _contexts.game.ReplaceMaxObjectLevel(levelData.maxObjectLevel);
    }

    private string GetRandomLevelName()
    {
        var currentLevel = _contexts.game.currentLevelIndex.Value;
        return "Level " + currentLevel;
    }
    
    public void ResumeGame()
    {
        SetLevelStatus(LevelStatus.Continue);
        _timeService.ResumeTime();
    }

    public void PauseGame()
    {
        SetLevelStatus(LevelStatus.Pause);
        _timeService.PauseTime();
    }

    public void RestartGame()
    {
        _contexts.game.CreateEntity().isLoadLevel = true;
        _contexts.game.CreateEntity().isLevelRestart = true;
    }

    public void NextLevel()
    {
        RestartGame();
    }
}
