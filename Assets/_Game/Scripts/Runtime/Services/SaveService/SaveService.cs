using UnityEngine;

public class SaveService : Service, ISaveService
{
    public SaveService(Contexts contexts) : base(contexts)
    {
    }

    public string CurrentLevelKey
    {
        get => "CurrentLevel";
    }

    public string TotalGoldKey 
    {
        get => "TotalGold";
    }
    public string AvailableObjectsKey 
    {
        get => "AvailableObjects";
    }

    public int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
}