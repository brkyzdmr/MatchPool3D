//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public AnyPurchaseActionListenerComponent anyPurchaseActionListener { get { return (AnyPurchaseActionListenerComponent)GetComponent(GameComponentsLookup.AnyPurchaseActionListener); } }
    public bool hasAnyPurchaseActionListener { get { return HasComponent(GameComponentsLookup.AnyPurchaseActionListener); } }

    public void AddAnyPurchaseActionListener(System.Collections.Generic.List<IAnyPurchaseActionListener> newValue) {
        var index = GameComponentsLookup.AnyPurchaseActionListener;
        var component = (AnyPurchaseActionListenerComponent)CreateComponent(index, typeof(AnyPurchaseActionListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAnyPurchaseActionListener(System.Collections.Generic.List<IAnyPurchaseActionListener> newValue) {
        var index = GameComponentsLookup.AnyPurchaseActionListener;
        var component = (AnyPurchaseActionListenerComponent)CreateComponent(index, typeof(AnyPurchaseActionListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAnyPurchaseActionListener() {
        RemoveComponent(GameComponentsLookup.AnyPurchaseActionListener);
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

    static Entitas.IMatcher<GameEntity> _matcherAnyPurchaseActionListener;

    public static Entitas.IMatcher<GameEntity> AnyPurchaseActionListener {
        get {
            if (_matcherAnyPurchaseActionListener == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AnyPurchaseActionListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAnyPurchaseActionListener = matcher;
            }

            return _matcherAnyPurchaseActionListener;
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

    public void AddAnyPurchaseActionListener(IAnyPurchaseActionListener value) {
        var listeners = hasAnyPurchaseActionListener
            ? anyPurchaseActionListener.value
            : new System.Collections.Generic.List<IAnyPurchaseActionListener>();
        listeners.Add(value);
        ReplaceAnyPurchaseActionListener(listeners);
    }

    public void RemoveAnyPurchaseActionListener(IAnyPurchaseActionListener value, bool removeComponentWhenEmpty = true) {
        var listeners = anyPurchaseActionListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0) {
            RemoveAnyPurchaseActionListener();
        } else {
            ReplaceAnyPurchaseActionListener(listeners);
        }
    }
}
