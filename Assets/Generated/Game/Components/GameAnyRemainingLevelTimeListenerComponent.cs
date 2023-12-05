//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public AnyRemainingLevelTimeListenerComponent anyRemainingLevelTimeListener { get { return (AnyRemainingLevelTimeListenerComponent)GetComponent(GameComponentsLookup.AnyRemainingLevelTimeListener); } }
    public bool hasAnyRemainingLevelTimeListener { get { return HasComponent(GameComponentsLookup.AnyRemainingLevelTimeListener); } }

    public void AddAnyRemainingLevelTimeListener(System.Collections.Generic.List<IAnyRemainingLevelTimeListener> newValue) {
        var index = GameComponentsLookup.AnyRemainingLevelTimeListener;
        var component = (AnyRemainingLevelTimeListenerComponent)CreateComponent(index, typeof(AnyRemainingLevelTimeListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAnyRemainingLevelTimeListener(System.Collections.Generic.List<IAnyRemainingLevelTimeListener> newValue) {
        var index = GameComponentsLookup.AnyRemainingLevelTimeListener;
        var component = (AnyRemainingLevelTimeListenerComponent)CreateComponent(index, typeof(AnyRemainingLevelTimeListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAnyRemainingLevelTimeListener() {
        RemoveComponent(GameComponentsLookup.AnyRemainingLevelTimeListener);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherAnyRemainingLevelTimeListener;

    public static Entitas.IMatcher<GameEntity> AnyRemainingLevelTimeListener {
        get {
            if (_matcherAnyRemainingLevelTimeListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AnyRemainingLevelTimeListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAnyRemainingLevelTimeListener = matcher;
            }

            return _matcherAnyRemainingLevelTimeListener;
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.EventEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public void AddAnyRemainingLevelTimeListener(IAnyRemainingLevelTimeListener value) {
        var listeners = hasAnyRemainingLevelTimeListener
            ? anyRemainingLevelTimeListener.value
            : new System.Collections.Generic.List<IAnyRemainingLevelTimeListener>();
        listeners.Add(value);
        ReplaceAnyRemainingLevelTimeListener(listeners);
    }

    public void RemoveAnyRemainingLevelTimeListener(IAnyRemainingLevelTimeListener value, bool removeComponentWhenEmpty = true) {
        var listeners = anyRemainingLevelTimeListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveAnyRemainingLevelTimeListener();
        } else {
            ReplaceAnyRemainingLevelTimeListener(listeners);
        }
    }
}
