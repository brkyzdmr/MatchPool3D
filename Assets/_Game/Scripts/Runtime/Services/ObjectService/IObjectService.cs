public interface IObjectService
{
    public ObjectsConfigData.ObjectData GetRandomAvailableObject(int availableObjectsBitmask);
    public string GetObjectPath(ObjectsConfigData.ObjectData objectData, int level);
    public string GetObjectPath(string type, int level);
}