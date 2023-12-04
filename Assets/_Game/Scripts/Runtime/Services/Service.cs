public abstract class Service : IService
{
    protected readonly Contexts Contexts;

    protected Service(Contexts contexts)
    {
        Contexts = contexts;
    }

    public virtual void DropState() { }
}