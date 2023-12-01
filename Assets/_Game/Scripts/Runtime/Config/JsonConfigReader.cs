using UnityEngine;
using System.IO;

public static class JsonConfigReader
{
    public static T ReadJsonConfig<T>(string path) where T : class
    {
        try
        {
            string json = Resources.Load<TextAsset>(path).text;
            return JsonUtility.FromJson<T>(json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error reading JSON config: " + ex.Message);
            return null;
        }
    }
}