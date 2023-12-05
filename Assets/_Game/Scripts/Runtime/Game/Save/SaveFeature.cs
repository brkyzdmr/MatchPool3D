
public class SaveFeature : Feature
{
    public SaveFeature(Contexts contexts)
    {
        Add(new SaveSystem(contexts));
        Add(new LoadSystem(contexts));
    }
}
