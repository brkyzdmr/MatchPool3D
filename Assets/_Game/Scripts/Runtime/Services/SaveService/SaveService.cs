using UnityEngine;

public class SaveService : Service, ISaveService
{
    private readonly Contexts _contexts;

    public SaveService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
    }
    
    public string CurrentLevelKey => "CurrentLevel";

    public string TotalGoldKey => "TotalGold";

    public string AvailableObjectsKey => "AvailableObjects";

    public int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public void Save()
    {
        Debug.Log("SaveService - Save");
        SetInt(CurrentLevelKey, _contexts.game.currentLevelIndex.Value);
        SetInt(TotalGoldKey, _contexts.game.totalGold.Value);
        SetInt(AvailableObjectsKey, _contexts.game.availableObjects.Value);
    }

    public void Load()
    {
        Debug.Log("SaveService - Load");
        _contexts.game.ReplaceCurrentLevelIndex(GetInt(CurrentLevelKey, 0));
        _contexts.game.ReplaceTotalGold(GetInt(TotalGoldKey, 0));
        _contexts.game.ReplaceAvailableObjects(GetInt(AvailableObjectsKey, 1));
    }
}