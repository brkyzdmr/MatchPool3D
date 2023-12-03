//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventSystemGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed class ObjectEventSystem : Entitas.ReactiveSystem<GameEntity> {

    readonly System.Collections.Generic.List<IObjectListener> _listenerBuffer;

    public ObjectEventSystem(Contexts contexts) : base(contexts.game) {
        _listenerBuffer = new System.Collections.Generic.List<IObjectListener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context) {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Added(GameMatcher.Object)
        );
    }

    protected override bool Filter(GameEntity entity) {
        return entity.hasObject && entity.hasObjectListener;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities) {
        foreach (var e in entities) {
            var component = e.@object;
            _listenerBuffer.Clear();
            _listenerBuffer.AddRange(e.objectListener.value);
            foreach (var listener in _listenerBuffer) {
                listener.OnObject(e, component.Type, component.Level);
            }
        }
    }
}
