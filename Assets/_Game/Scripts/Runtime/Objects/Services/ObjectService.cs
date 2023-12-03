using System.Collections.Generic;

public static class ObjectService
{
    public static readonly IObjectsConfig ObjectsConfig = Contexts.sharedInstance.config.objectsConfig.value;

    public static readonly Dictionary<string, ObjectsConfigData.ObjectData> ObjectDatas =
        new Dictionary<string, ObjectsConfigData.ObjectData>();
    
    static ObjectService()
    {
        foreach (var objectData in ObjectsConfig.Config.objects)
        {
            ObjectDatas[objectData.type] = objectData;
        }
    }

    public static ObjectsConfigData.ObjectData GetRandomAvailableObject()
    {
        int availableObjectsBitmask = LevelService.AvailableObjects;
        List<ObjectsConfigData.ObjectData> availableObjects = new List<ObjectsConfigData.ObjectData>();

        var objectsCount = ObjectsConfig.Config.objects.Count;

        for (int i = 0; i < objectsCount; i++)
        {
            // Check if the object is available 
            if ((availableObjectsBitmask & (1 << i)) != 0)
            {
                availableObjects.Add(ObjectsConfig.Config.objects[i]);
            }
        }

        if (availableObjects.Count == 0)
        {
            return null;
        }

        return availableObjects[UnityEngine.Random.Range(0, availableObjects.Count)];
    }

    public static string GetObjectPath(ObjectsConfigData.ObjectData objectData, int level)
    {
        return objectData.levels[level];
    }

    public static string GetObjectPath(string type, int level)
    {
        return ObjectDatas[type].levels[level];
    }
}