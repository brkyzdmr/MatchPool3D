using System.Collections.Generic;
using UnityEngine;

public interface IObjectService
{
    public List<ObjectsConfigData.ObjectData> GetAllAvailableObjects();
    public ObjectsConfigData.ObjectData GetRandomAvailableObject();
    public void SetAvailableObjectByType(string objectType, bool isAvailable);
    public string GetObjectPath(ObjectsConfigData.ObjectData objectData, int level);
    public string GetObjectPath(string type, int level);
    public Sprite GetObjectSpriteByType(string type);
}