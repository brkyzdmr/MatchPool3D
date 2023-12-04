public sealed class InputFeature : Feature
{
    public InputFeature(Contexts contexts)
    {
        Add(new InputSystem(contexts));
    }
}

