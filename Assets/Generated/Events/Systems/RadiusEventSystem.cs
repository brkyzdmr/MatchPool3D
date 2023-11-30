//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventSystemGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class RadiusEventSystem : Entitas.ReactiveSystem<GameEntity> {

    readonly System.Collections.Generic.List<IRadiusListener> _listenerBuffer;

    public RadiusEventSystem(Contexts contexts) : base(contexts.game) {
        _listenerBuffer = new System.Collections.Generic.List<IRadiusListener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context) {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Added(GameMatcher.Radius)
        );
    }

    protected override bool Filter(GameEntity entity) {
        return entity.hasRadius && entity.hasRadiusListener;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities) {
        foreach (var e in entities) {
            var component = e.radius;
            _listenerBuffer.Clear();
            _listenerBuffer.AddRange(e.radiusListener.value);
            foreach (var listener in _listenerBuffer) {
                listener.OnRadius(e, component.Value);
            }
        }
    }
}
