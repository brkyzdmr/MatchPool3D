public sealed class TimeFeature : Feature
{
    public TimeFeature(Contexts contexts)
    {
        Add(new TickCurrentTimeSystem(contexts));
    }
}
