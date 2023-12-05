//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity currentLevelNameEntity { get { return GetGroup(GameMatcher.CurrentLevelName).GetSingleEntity(); } }
    public CurrentLevelNameComponent currentLevelName { get { return currentLevelNameEntity.currentLevelName; } }
    public bool hasCurrentLevelName { get { return currentLevelNameEntity != null; } }

    public GameEntity SetCurrentLevelName(string newValue) {
        if (hasCurrentLevelName) {
            throw new Entitas.EntitasException("Could not set CurrentLevelName!\n" + this + " already has an entity with CurrentLevelNameComponent!",
                "You should check if the context already has a currentLevelNameEntity before setting it or use context.ReplaceCurrentLevelName().");
        }
        var entity = CreateEntity();
        entity.AddCurrentLevelName(newValue);
        return entity;
    }

    public void ReplaceCurrentLevelName(string newValue) {
        var entity = currentLevelNameEntity;
        if (entity == null) {
            entity = SetCurrentLevelName(newValue);
        } else {
            entity.ReplaceCurrentLevelName(newValue);
        }
    }

    public void RemoveCurrentLevelName() {
        currentLevelNameEntity.Destroy();
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public CurrentLevelNameComponent currentLevelName { get { return (CurrentLevelNameComponent)GetComponent(GameComponentsLookup.CurrentLevelName); } }
    public bool hasCurrentLevelName { get { return HasComponent(GameComponentsLookup.CurrentLevelName); } }

    public void AddCurrentLevelName(string newValue) {
        var index = GameComponentsLookup.CurrentLevelName;
        var component = (CurrentLevelNameComponent)CreateComponent(index, typeof(CurrentLevelNameComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceCurrentLevelName(string newValue) {
        var index = GameComponentsLookup.CurrentLevelName;
        var component = (CurrentLevelNameComponent)CreateComponent(index, typeof(CurrentLevelNameComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveCurrentLevelName() {
        RemoveComponent(GameComponentsLookup.CurrentLevelName);
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

    static Entitas.IMatcher<GameEntity> _matcherCurrentLevelName;

    public static Entitas.IMatcher<GameEntity> CurrentLevelName {
        get {
            if (_matcherCurrentLevelName == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.CurrentLevelName);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherCurrentLevelName = matcher;
            }

            return _matcherCurrentLevelName;
        }
    }
}