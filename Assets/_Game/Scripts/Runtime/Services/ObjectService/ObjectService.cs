using System.Collections.Generic;

public class ObjectService : Service, IObjectService
{
    private readonly IObjectsConfig _objectsConfig;
    private readonly Dictionary<string, ObjectsConfigData.ObjectData> _objectDatas;
    private readonly Contexts _contexts;


    public ObjectService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
        _objectsConfig = contexts.config.objectsConfig.value;
        _objectDatas = new Dictionary<string, ObjectsConfigData.ObjectData>();

        foreach (var objectData in _objectsConfig.Config.objects)
        {
            _objectDatas[objectData.type] = objectData;
        }
    }

    public ObjectsConfigData.ObjectData GetRandomAvailableObject(int availableObjectsBitmask)
    {
        List<ObjectsConfigData.ObjectData> availableObjects = new List<ObjectsConfigData.ObjectData>();

        for (int i = 0; i < _objectsConfig.Config.objects.Count; i++)
        {
            if ((availableObjectsBitmask & (1 << i)) != 0)
            {
                availableObjects.Add(_objectsConfig.Config.objects[i]);
            }
        }

        return availableObjects.Count == 0 ? null : availableObjects[UnityEngine.Random.Range(0, availableObjects.Count)];
    }

    public string GetObjectPath(ObjectsConfigData.ObjectData objectData, int level)
    {
        return objectData.levels[level];
    }

    public string GetObjectPath(string type, int level)
    {
        return _objectDatas[type].levels[level];
    }
}