﻿
public sealed class ObjectFeature : Feature
{
    public ObjectFeature(Contexts contexts)
    {
        Add(new ObjectProductionSystem(contexts));
    }
}
