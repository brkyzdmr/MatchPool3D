
public class PhysicSystems : Feature
{
    public PhysicSystems(Contexts contexts, Services services)
    {
        Add(new SyncPositionSystem(contexts));
        Add(new PhysicsUpdateSystem(contexts));
        Add(new ApplyForceSystem(contexts));
        Add(new CollisionDetectionSystem(contexts));
        Add(new CollisionResponseSystem(contexts));

    }
}
