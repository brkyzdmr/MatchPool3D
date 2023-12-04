public sealed class InputFeature : Feature
{
    public InputFeature(Contexts contexts, Services services)
    {
        Add(new InputSystem(contexts, services));
    }
}

