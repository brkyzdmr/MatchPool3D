
using System.Collections.Generic;

public interface IObjectProductionService
{
    public List<(ObjectsConfigData.ObjectData, int, int)> GenerateObjects(
        List<ObjectsConfigData.ObjectData> availableObjects);
}
