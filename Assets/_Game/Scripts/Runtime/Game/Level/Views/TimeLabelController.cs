using System;
using TMPro;
using UnityEngine;

public class TimeLabelController : MonoBehaviour, IAnyTimeTickListener, IAnyLevelReadyListener, IAnyRemainingLevelTimeListener
{
    [SerializeField] private TMP_Text label;
    private float _startTime;
    private float _passedTime;
    private Contexts _contexts;
    private GameEntity _listener;
    private ILevelService _levelService;
    private ITimeService _timeService;

    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _levelService = Services.GetService<ILevelService>();
        _timeService = Services.GetService<ITimeService>();
        
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyTimeTickListener(this);
        _listener.AddAnyLevelReadyListener(this);
        _listener.AddAnyRemainingLevelTimeListener(this);
    }

    public void OnAnyTimeTick(GameEntity entity)
    {
        _passedTime++;
        var remainingTime = _startTime - _passedTime;
        _contexts.game.ReplaceRemainingLevelTime((int) remainingTime);
        label.text = _timeService.FormatTimeDuration(remainingTime);

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

        var time = _contexts.game.levelDuration.Value;
        _startTime = time;
        _passedTime = 0;
        
        label.text = _timeService.FormatTimeDuration(time);
    }

    public void OnAnyRemainingLevelTime(GameEntity entity, int value)
    {
        label.text = _timeService.FormatTimeDuration(value);
    }
}
