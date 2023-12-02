using System.Collections.Generic;

public static class ObjectService
{
    public static readonly IObjectsConfig ObjectsConfig = Contexts.sharedInstance.config.objectsConfig.value;

    public static string GetRandomAvailableObjectPath(int level)
    {
        int availableObjectsBitmask = LevelService.AvailableObjects;
        List<string> availableObjectPaths = new List<string>();

        var objectsCount = ObjectsConfig.Config.objects.Count;
        
        for (int i = 0; i < objectsCount; i++)
        {
            // Check if the object is available 
            if ((availableObjectsBitmask & (1 << i)) != 0)
            {
                availableObjectPaths.Add(ObjectsConfig.Config.objects[i].levels[level - 1]);
            }
        }

        return availableObjectPaths[UnityEngine.Random.Range(0, availableObjectPaths.Count)];
    }
}