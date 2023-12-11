public interface ISaveService
{
    public string CurrentLevelKey { get; }
    public string TotalGoldKey { get; }
    public string AvailableObjectsKey { get; }
    public int GetInt(string key, int defaultValue);
    public void SetInt(string key, int value);

    public void Save();
    public void Load();
}