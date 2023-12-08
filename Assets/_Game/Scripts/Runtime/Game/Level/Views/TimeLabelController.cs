using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TimeLabelController : MonoBehaviour, IAnyTimeTickListener, IAnyLevelReadyListener, IAnyRemainingLevelTimeListener
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private AnimationCurve countdownAnimationCurve;
    
    private float _startTime;
    private float _passedTime;
    private Contexts _contexts;
    private GameEntity _listener;
    private ILevelService _levelService;
    private ITimeService _timeService;
    private Color _defaultColor;
    private Tween _countdownTween;

    void Start()
    {
        InitializeServices();
        RegisterListeners();
        _defaultColor = label.color;
    }

    private void InitializeServices()
    {
        _contexts = Contexts.sharedInstance;
        _levelService = Services.GetService<ILevelService>();
        _timeService = Services.GetService<ITimeService>();
    }

    private void RegisterListeners()
    {
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyTimeTickListener(this);
        _listener.AddAnyLevelReadyListener(this);
        _listener.AddAnyRemainingLevelTimeListener(this);
    }

    public void OnAnyTimeTick(GameEntity entity)
    {
        UpdatePassedTime();
        HandleTimeUpdate();
        CheckLastSeconds();
    }

    private void CheckLastSeconds()
    {
        var remainingTime = _contexts.game.remainingLevelTime.Value;
        
        if (remainingTime >= 0 && remainingTime < 4)
        {
            ChangeLabelColor(Color.red);
            AnimateLabel();
        }
    }

    private void AnimateLabel()
    {
        _countdownTween?.Kill();
        _countdownTween = label.DOScale(1.35f, 0.58f)
            .SetEase(countdownAnimationCurve)
            .SetUpdate(true);
    }

    public void OnAnyLevelReady(GameEntity entity)
    {
        if (!_contexts.game.isLevelReady)
            return;
        
        if(!_listener.hasAnyTimeTickListener)
            _listener.AddAnyTimeTickListener(this);

        ChangeLabelColor(_defaultColor);
        SetupTimeForLevel();
    }

    public void OnAnyRemainingLevelTime(GameEntity entity, int value)
    {
        UpdateLabel(value);
    }

    private void UpdatePassedTime()
    {
        _passedTime++;
    }

    private void HandleTimeUpdate()
    {
        var remainingTime = _startTime - _passedTime;
        _contexts.game.ReplaceRemainingLevelTime((int)remainingTime);

        UpdateLabel(remainingTime);
        CheckTimeCompletion(remainingTime);
    }

    private void ChangeLabelColor(Color color)
    {
        label.color = color;
    }

    private void UpdateLabel(float value)
    {
        label.text = value < 0 ? "∞" : _timeService.FormatTimeDuration(value);
    }

    private void CheckTimeCompletion(float remainingTime)
    {
        if (Math.Abs(_passedTime - _startTime) < 0.1f)
        {
            SetGameEndState();
            _listener.RemoveAnyTimeTickListener(this);
        }
    }

    private void SetGameEndState()
    {
        _contexts.input.isInputBlock = true;
        _contexts.game.isLevelReady = false;
        _contexts.game.isLevelEnd = true;
    }

    private void SetupTimeForLevel()
    {
        var time = _contexts.game.levelDuration.Value;

        _startTime = time;
        _passedTime = 0;

        UpdateLabel(time);
    }
}
