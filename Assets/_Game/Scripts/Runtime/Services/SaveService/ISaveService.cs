public interface ISaveService
{
    public int GetInt(string key, int defaultValue);
    public void SetInt(string key, int value);
}