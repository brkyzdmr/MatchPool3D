using System.Collections.Generic;
using Entitas;
using Game;
using UnityEngine;

public sealed class InitializeLevelSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    readonly Contexts _contexts;
    Transform _playAreaContainer;
    private int _levelCount;
    
    public InitializeLevelSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        // var levelInfo = Resources.Load<TextAsset>(LevelImportTags.LevelInfo).text;
        // _levelCount = JsonUtility.FromJson<LevelInfo>(levelInfo).LevelCount;
        //
        SetupLevel();
    }

    private void SetupLevel()
    {
        if (LevelService.PlayerCurrentLevel >= _levelCount)
        {
            LevelService.PlayerCurrentLevel = Random.Range(0, _levelCount);
        }
        
        var level = LevelService.PlayerCurrentLevel;
        _contexts.input.isInputBlock = true;
        // var levelString = Resources.Load<TextAsset>(LevelImportTags.GetLevelName(level)).text;
        // var data = JsonUtility.FromJson<LevelDataContainer>(levelString);
        //
        // CreateCubes(data, out var averagePos);
        // CreatePlayAreaContainer(averagePos);
        // CreatePlayer();
        // CreateMagneticField();
        CreateObjects();
        _contexts.game.isLevelReady = true;
        _contexts.input.isInputBlock = false;
    }

    private void CreatePlayAreaContainer(Vector3 averagePos)
    {
        // if (_playAreaContainer != null)
        // {
        //     _playAreaContainer.DetachChildren();
        //     GameObject.Destroy(_playAreaContainer.gameObject);
        // }
        //
        // _playAreaContainer = new GameObject("PlayAreaContainer").transform;
        // var levelConfig = _contexts.config.levelConfig.value;
        // _playAreaContainer.position = new Vector3(-averagePos.x + levelConfig.PlayAreaContainerOffset.x,
        //     0 + levelConfig.PlayAreaContainerOffset.y, -averagePos.y + levelConfig.PlayAreaContainerOffset.z);
        // _playAreaContainer.rotation = levelConfig.PlayAreaContainerRotation.GetEulerQuaternion();
        // _contexts.game.ReplacePlayAreaParent(_playAreaContainer);
    }

    private void CreateObjects()
    {
        var cubeCount = 0;

        for (int i = 0; i < 10; i++)
        {
            var randomPosition = new Vector3(Random.Range(-5, 5), 10, Random.Range(-5, 5));
            _contexts.game.CreateObject(randomPosition);
        }
        
        _contexts.game.ReplaceCreatedObjectsCount(cubeCount);
    }


    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(GameMatcher.LoadLevel);
    
    protected override bool Filter(GameEntity entity) => _contexts.game.isLevelEnd;
    
    protected override void Execute(List<GameEntity> entities)
    {
        // var cubes = _contexts.game.GetEntities(GameMatcher.ColoredCube);
        // foreach (var gameEntity in cubes)
        // {
        //     gameEntity.isDestroyed = true;
        // }
        // var player = _contexts.game.GetEntities(GameMatcher.Player);
        // foreach (var gameEntity in player)
        // {
        //     gameEntity.isDestroyed = true;
        // }
        //
        // _contexts.game.isLevelEnd = false;
        // // _contexts.game.ReplaceCurrentCollectedCubes(0);
        // // _contexts.game.ReplaceCreatedCubeCount(0);
        // SetupLevel();
    }
}
