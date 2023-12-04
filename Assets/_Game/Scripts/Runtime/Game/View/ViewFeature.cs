public sealed class ViewFeature : Feature
{
    public ViewFeature(Contexts contexts)
    {
        Add(new AddViewSystem(contexts));
    }
}

