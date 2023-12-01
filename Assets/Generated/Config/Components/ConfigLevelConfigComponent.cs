//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ConfigContext {

    public ConfigEntity levelConfigEntity { get { return GetGroup(ConfigMatcher.LevelConfig).GetSingleEntity(); } }
    public LevelConfigComponent levelConfig { get { return levelConfigEntity.levelConfig; } }
    public bool hasLevelConfig { get { return levelConfigEntity != null; } }

    public ConfigEntity SetLevelConfig(ILevelsConfig newValue) {
        if (hasLevelConfig) {
            throw new Entitas.EntitasException("Could not set LevelConfig!\n" + this + " already has an entity with LevelConfigComponent!",
                "You should check if the context already has a levelConfigEntity before setting it or use context.ReplaceLevelConfig().");
        }
        var entity = CreateEntity();
        entity.AddLevelConfig(newValue);
        return entity;
    }

    public void ReplaceLevelConfig(ILevelsConfig newValue) {
        var entity = levelConfigEntity;
        if (entity == null) {
            entity = SetLevelConfig(newValue);
        } else {
            entity.ReplaceLevelConfig(newValue);
        }
    }

    public void RemoveLevelConfig() {
        levelConfigEntity.Destroy();
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
public partial class ConfigEntity {

    public LevelConfigComponent levelConfig { get { return (LevelConfigComponent)GetComponent(ConfigComponentsLookup.LevelConfig); } }
    public bool hasLevelConfig { get { return HasComponent(ConfigComponentsLookup.LevelConfig); } }

    public void AddLevelConfig(ILevelsConfig newValue) {
        var index = ConfigComponentsLookup.LevelConfig;
        var component = (LevelConfigComponent)CreateComponent(index, typeof(LevelConfigComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceLevelConfig(ILevelsConfig newValue) {
        var index = ConfigComponentsLookup.LevelConfig;
        var component = (LevelConfigComponent)CreateComponent(index, typeof(LevelConfigComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveLevelConfig() {
        RemoveComponent(ConfigComponentsLookup.LevelConfig);
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
public sealed partial class ConfigMatcher {

    static Entitas.IMatcher<ConfigEntity> _matcherLevelConfig;

    public static Entitas.IMatcher<ConfigEntity> LevelConfig {
        get {
            if (_matcherLevelConfig == null) {
                var matcher = (Entitas.Matcher<ConfigEntity>)Entitas.Matcher<ConfigEntity>.AllOf(ConfigComponentsLookup.LevelConfig);
                matcher.componentNames = ConfigComponentsLookup.componentNames;
                _matcherLevelConfig = matcher;
            }

            return _matcherLevelConfig;
        }
    }
}
