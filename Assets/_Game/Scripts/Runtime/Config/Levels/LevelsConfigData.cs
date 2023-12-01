using System.Collections.Generic;

[System.Serializable]
public class LevelsConfigData
{
    public List<LevelData> levels;

    [System.Serializable]
    public class LevelData
    {
        public string name;
        public string type;
        public int duration;
        public int maxObjectLevel;
        public int maxProducedObjectLevel;
        public int maxProducedObjectCount;
    }
}