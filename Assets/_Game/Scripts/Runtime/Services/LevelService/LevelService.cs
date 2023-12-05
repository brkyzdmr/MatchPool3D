﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelService : Service, ILevelService
{
    private readonly Contexts _contexts;
    
    public LevelService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
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

    public void RefreshData()
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
}