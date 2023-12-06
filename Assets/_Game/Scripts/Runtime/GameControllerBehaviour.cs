using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameControllerBehaviour : MonoBehaviour
{
    private Contexts _contexts;
    private GameController _gameController;

    void Awake()
    {
        _contexts = Contexts.sharedInstance;

        LoadConfigurations();
        CreateServices();
        Configure();
    }

    public void Start()
    {
        _gameController.Initialize();
    }

    public void Update() => _gameController.Execute();

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            SaveGameData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void LoadConfigurations()
    {
        _contexts.config.ReplaceObjectsConfig(ObjectsConfigManager.LoadGameConfig());
        _contexts.config.ReplaceGameConfig(GameConfigManager.LoadGameConfig());
        _contexts.config.ReplaceLevelsConfig(LevelsConfigManager.LoadLevelsConfig());
    }

    private void CreateServices()
    {
        Services.RegisterService<ISaveService>(new SaveService(_contexts));
        Services.RegisterService<IGameService>(new GameService(_contexts));
        Services.RegisterService<ILevelService>(new LevelService(_contexts));
        Services.RegisterService<IInputService>(new UnityInputService(_contexts));
        Services.RegisterService<IObjectService>(new ObjectService(_contexts));
        Services.RegisterService<ITimeService>(new TimeService(_contexts));
        Services.RegisterService<IShopService>(new ShopService(_contexts));
    }

    private void Configure()
    {
        DOTween.SetTweensCapacity(500, 50);
        _gameController = new GameController(_contexts);
    }
    

    private void SaveGameData()
    {
        _contexts.game.isSave = true;
    }
}
