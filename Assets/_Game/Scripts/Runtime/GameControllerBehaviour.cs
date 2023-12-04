using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameControllerBehaviour : MonoBehaviour
{
    private Contexts _contexts;
    private Services _services;
    private GameController _gameController;

    void Awake()
    {
        _contexts = Contexts.sharedInstance;
        _services = new Services();
        
        LoadConfigurations();
        CreateServices();
        Configure();
    }

    public void Start() => _gameController.Initialize();
    public void Update() => _gameController.Execute();
    private void LoadConfigurations()
    {
        _contexts.config.ReplaceObjectsConfig(ObjectsConfigManager.LoadGameConfig());
        _contexts.config.ReplaceGameConfig(GameConfigManager.LoadGameConfig());
        _contexts.config.ReplaceLevelsConfig(LevelsConfigManager.LoadLevelsConfig());
    }

    private void CreateServices()
    {
        _services.SaveService = new SaveService(_contexts);
        _services.LevelService = new LevelService(_contexts, _services);
        _services.InputService = new UnityInputService(_contexts);
        _services.ObjectService = new ObjectService(_contexts);
    }

    private void Configure()
    {
        DOTween.SetTweensCapacity(500, 50);
        _gameController = new GameController(_contexts, _services);
    }
}
