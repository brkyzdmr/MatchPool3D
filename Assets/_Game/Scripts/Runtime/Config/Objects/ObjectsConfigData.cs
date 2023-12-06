using System.Collections.Generic;

[System.Serializable]
public class ObjectsConfigData
{
    public List<ObjectData> objects;

    [System.Serializable]
    public class ObjectData
    {
        public string type;
        public ShopData shop;
        public List<string> levels;

        [System.Serializable]
        public class ShopData
        {
            public string name;
            public string sprite;
            public int price;
        }
    }
}