public abstract class Service
{
    protected readonly Contexts Contexts;

    protected Service(Contexts contexts)
    {
        Contexts = contexts;
    }

    protected virtual void DropState()
    {
    }
}