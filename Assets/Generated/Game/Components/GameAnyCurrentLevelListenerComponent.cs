//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public AnyCurrentLevelListenerComponent anyCurrentLevelListener { get { return (AnyCurrentLevelListenerComponent)GetComponent(GameComponentsLookup.AnyCurrentLevelListener); } }
    public bool hasAnyCurrentLevelListener { get { return HasComponent(GameComponentsLookup.AnyCurrentLevelListener); } }

    public void AddAnyCurrentLevelListener(System.Collections.Generic.List<IAnyCurrentLevelListener> newValue) {
        var index = GameComponentsLookup.AnyCurrentLevelListener;
        var component = (AnyCurrentLevelListenerComponent)CreateComponent(index, typeof(AnyCurrentLevelListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAnyCurrentLevelListener(System.Collections.Generic.List<IAnyCurrentLevelListener> newValue) {
        var index = GameComponentsLookup.AnyCurrentLevelListener;
        var component = (AnyCurrentLevelListenerComponent)CreateComponent(index, typeof(AnyCurrentLevelListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAnyCurrentLevelListener() {
        RemoveComponent(GameComponentsLookup.AnyCurrentLevelListener);
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

    static Entitas.IMatcher<GameEntity> _matcherAnyCurrentLevelListener;

    public static Entitas.IMatcher<GameEntity> AnyCurrentLevelListener {
        get {
            if (_matcherAnyCurrentLevelListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AnyCurrentLevelListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAnyCurrentLevelListener = matcher;
            }

            return _matcherAnyCurrentLevelListener;
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

    public void AddAnyCurrentLevelListener(IAnyCurrentLevelListener value) {
        var listeners = hasAnyCurrentLevelListener
            ? anyCurrentLevelListener.value
            : new System.Collections.Generic.List<IAnyCurrentLevelListener>();
        listeners.Add(value);
        ReplaceAnyCurrentLevelListener(listeners);
    }

    public void RemoveAnyCurrentLevelListener(IAnyCurrentLevelListener value, bool removeComponentWhenEmpty = true) {
        var listeners = anyCurrentLevelListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveAnyCurrentLevelListener();
        } else {
            ReplaceAnyCurrentLevelListener(listeners);
        }
    }
}
