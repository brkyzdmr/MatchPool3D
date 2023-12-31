//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public AnyLevelEndListenerComponent anyLevelEndListener { get { return (AnyLevelEndListenerComponent)GetComponent(GameComponentsLookup.AnyLevelEndListener); } }
    public bool hasAnyLevelEndListener { get { return HasComponent(GameComponentsLookup.AnyLevelEndListener); } }

    public void AddAnyLevelEndListener(System.Collections.Generic.List<IAnyLevelEndListener> newValue) {
        var index = GameComponentsLookup.AnyLevelEndListener;
        var component = (AnyLevelEndListenerComponent)CreateComponent(index, typeof(AnyLevelEndListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAnyLevelEndListener(System.Collections.Generic.List<IAnyLevelEndListener> newValue) {
        var index = GameComponentsLookup.AnyLevelEndListener;
        var component = (AnyLevelEndListenerComponent)CreateComponent(index, typeof(AnyLevelEndListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAnyLevelEndListener() {
        RemoveComponent(GameComponentsLookup.AnyLevelEndListener);
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

    static Entitas.IMatcher<GameEntity> _matcherAnyLevelEndListener;

    public static Entitas.IMatcher<GameEntity> AnyLevelEndListener {
        get {
            if (_matcherAnyLevelEndListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AnyLevelEndListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAnyLevelEndListener = matcher;
            }

            return _matcherAnyLevelEndListener;
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

    public void AddAnyLevelEndListener(IAnyLevelEndListener value) {
        var listeners = hasAnyLevelEndListener
            ? anyLevelEndListener.value
            : new System.Collections.Generic.List<IAnyLevelEndListener>();
        listeners.Add(value);
        ReplaceAnyLevelEndListener(listeners);
    }

    public void RemoveAnyLevelEndListener(IAnyLevelEndListener value, bool removeComponentWhenEmpty = true) {
        var listeners = anyLevelEndListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveAnyLevelEndListener();
        } else {
            ReplaceAnyLevelEndListener(listeners);
        }
    }
}
