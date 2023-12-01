//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public AnyLevelStatusListenerComponent anyLevelStatusListener { get { return (AnyLevelStatusListenerComponent)GetComponent(GameComponentsLookup.AnyLevelStatusListener); } }
    public bool hasAnyLevelStatusListener { get { return HasComponent(GameComponentsLookup.AnyLevelStatusListener); } }

    public void AddAnyLevelStatusListener(System.Collections.Generic.List<IAnyLevelStatusListener> newValue) {
        var index = GameComponentsLookup.AnyLevelStatusListener;
        var component = (AnyLevelStatusListenerComponent)CreateComponent(index, typeof(AnyLevelStatusListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAnyLevelStatusListener(System.Collections.Generic.List<IAnyLevelStatusListener> newValue) {
        var index = GameComponentsLookup.AnyLevelStatusListener;
        var component = (AnyLevelStatusListenerComponent)CreateComponent(index, typeof(AnyLevelStatusListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAnyLevelStatusListener() {
        RemoveComponent(GameComponentsLookup.AnyLevelStatusListener);
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

    static Entitas.IMatcher<GameEntity> _matcherAnyLevelStatusListener;

    public static Entitas.IMatcher<GameEntity> AnyLevelStatusListener {
        get {
            if (_matcherAnyLevelStatusListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AnyLevelStatusListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAnyLevelStatusListener = matcher;
            }

            return _matcherAnyLevelStatusListener;
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

    public void AddAnyLevelStatusListener(IAnyLevelStatusListener value) {
        var listeners = hasAnyLevelStatusListener
            ? anyLevelStatusListener.value
            : new System.Collections.Generic.List<IAnyLevelStatusListener>();
        listeners.Add(value);
        ReplaceAnyLevelStatusListener(listeners);
    }

    public void RemoveAnyLevelStatusListener(IAnyLevelStatusListener value, bool removeComponentWhenEmpty = true) {
        var listeners = anyLevelStatusListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveAnyLevelStatusListener();
        } else {
            ReplaceAnyLevelStatusListener(listeners);
        }
    }
}
