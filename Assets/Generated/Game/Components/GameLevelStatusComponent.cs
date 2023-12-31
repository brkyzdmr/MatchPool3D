//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity levelStatusEntity { get { return GetGroup(GameMatcher.LevelStatus).GetSingleEntity(); } }
    public LevelStatusComponent levelStatus { get { return levelStatusEntity.levelStatus; } }
    public bool hasLevelStatus { get { return levelStatusEntity != null; } }

    public GameEntity SetLevelStatus(LevelStatus newValue) {
        if (hasLevelStatus) {
            throw new Entitas.EntitasException("Could not set LevelStatus!\n" + this + " already has an entity with LevelStatusComponent!",
                "You should check if the context already has a levelStatusEntity before setting it or use context.ReplaceLevelStatus().");
        }
        var entity = CreateEntity();
        entity.AddLevelStatus(newValue);
        return entity;
    }

    public void ReplaceLevelStatus(LevelStatus newValue) {
        var entity = levelStatusEntity;
        if (entity == null) {
            entity = SetLevelStatus(newValue);
        } else {
            entity.ReplaceLevelStatus(newValue);
        }
    }

    public void RemoveLevelStatus() {
        levelStatusEntity.Destroy();
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

    public LevelStatusComponent levelStatus { get { return (LevelStatusComponent)GetComponent(GameComponentsLookup.LevelStatus); } }
    public bool hasLevelStatus { get { return HasComponent(GameComponentsLookup.LevelStatus); } }

    public void AddLevelStatus(LevelStatus newValue) {
        var index = GameComponentsLookup.LevelStatus;
        var component = (LevelStatusComponent)CreateComponent(index, typeof(LevelStatusComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceLevelStatus(LevelStatus newValue) {
        var index = GameComponentsLookup.LevelStatus;
        var component = (LevelStatusComponent)CreateComponent(index, typeof(LevelStatusComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveLevelStatus() {
        RemoveComponent(GameComponentsLookup.LevelStatus);
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

    static Entitas.IMatcher<GameEntity> _matcherLevelStatus;

    public static Entitas.IMatcher<GameEntity> LevelStatus {
        get {
            if (_matcherLevelStatus == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.LevelStatus);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherLevelStatus = matcher;
            }

            return _matcherLevelStatus;
        }
    }
}
