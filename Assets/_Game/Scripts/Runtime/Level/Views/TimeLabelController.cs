using System;
using Entitas;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TimeLabelController : MonoBehaviour, IAnyTimeTickListener, IAnyLevelReadyListener
{
    [SerializeField] private TMP_Text label;
    private float _startTime;
    private float _passedTime;
    private Contexts _contexts;
    private GameEntity _listener;

    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyTimeTickListener(this);
        _listener.AddAnyLevelReadyListener(this);
    }

    public void OnAnyTimeTick(GameEntity entity)
    {
        _passedTime++;
        UpdateLabelTime(_startTime - _passedTime);

        if (Math.Abs(_passedTime - _startTime) < 0.1f)
        {
            _contexts.input.isInputBlock = true;
            _contexts.game.isLevelReady = false;
            _contexts.game.isLevelEnd = true;
            _listener.RemoveAnyTimeTickListener(this);
        }
    }

    public void OnAnyLevelReady(GameEntity entity)
    {
        if (!_contexts.game.isLevelReady)
            return;

        if(!_listener.hasAnyTimeTickListener)
            _listener.AddAnyTimeTickListener(this);

        var levelConfig = _contexts.config.levelConfig.value;
        var level = LevelService.PlayerCurrentLevel;
        var time = levelConfig.Levels.levels[level].duration;
        _startTime = time;
        _passedTime = 0;

        UpdateLabelTime(time);
    }

    private void UpdateLabelTime(float timeInSeconds)
    {
        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds % 60;
        label.text = $"{minutes:00}:{seconds:00}";
    }
}
