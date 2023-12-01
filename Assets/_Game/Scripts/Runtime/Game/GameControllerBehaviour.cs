using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameControllerBehaviour : MonoBehaviour
{
    GameController _gameController;

    void Awake()
    {
        DOTween.SetTweensCapacity(500, 50);
        _gameController = new GameController(Contexts.sharedInstance);
        _gameController.ReplaceObjectsConfig(ObjectsConfigManager.LoadGameConfig());
        _gameController.ReplaceGameConfig(GameConfigManager.LoadGameConfig());
        _gameController.ReplaceLevelsConfig(LevelsConfigManager.LoadGameConfig());
    }

    void Start() => _gameController.Initialize();
    void Update() => _gameController.Execute();
}
