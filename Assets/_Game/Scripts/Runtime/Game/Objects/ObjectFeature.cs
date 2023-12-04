
public sealed class ObjectFeature : Feature
{
    public ObjectFeature(Contexts contexts, Services services)
    {
        Add(new ObjectProductionSystem(contexts, services));
        Add(new ObjectMergeSystem(contexts, services));
    }
}
