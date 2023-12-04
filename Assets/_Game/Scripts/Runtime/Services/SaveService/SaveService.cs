using UnityEngine;

public class SaveService : Service, ISaveService
{
    public const string CurrentLevelKey = "CurrentLevel";
    public const string TotalGoldKey = "TotalGold";
    public const string AvailableObjectsKey = "AvailableObjects";

    public SaveService(Contexts contexts) : base(contexts)
    {
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