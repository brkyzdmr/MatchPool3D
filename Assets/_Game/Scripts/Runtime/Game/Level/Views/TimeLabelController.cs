using System;
using TMPro;
using UnityEngine;

public class TimeLabelController : MonoBehaviour, IAnyTimeTickListener, IAnyLevelReadyListener
{
    [SerializeField] private TMP_Text label;
    private float _startTime;
    private float _passedTime;
    private Contexts _contexts;
    private GameEntity _listener;
    private ILevelService _levelService;

    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _levelService = Services.GetService<ILevelService>();
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyTimeTickListener(this);
        _listener.AddAnyLevelReadyListener(this);
    }

    public void OnAnyTimeTick(GameEntity entity)
    {
        _passedTime++;
        label.text = TimeService.FormatTimeDuration(_startTime - _passedTime);

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
        
        var levelConfig = _levelService.LevelsConfig;
        var level = _levelService.CurrentLevel;
        var time = levelConfig.Levels.levels[level].duration;
        _startTime = time;
        _passedTime = 0;
        
        label.text = TimeService.FormatTimeDuration(time);
    }
}
