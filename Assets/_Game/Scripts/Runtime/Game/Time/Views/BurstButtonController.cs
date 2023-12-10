using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

public class BurstButtonController : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private AnimationCurve tapAnimationCurve;
    
    private ITimeService _timeService;
    private IVibrationService _vibrationService;
    private float _defaultSpeed;
    private float _maxSpeed;
    private float _currentSpeed;
    private Button _burstButton;
    private Tween _speedTween;
    private Tween _tapAnimationTween;

    private void Start()
    {
        InitializeServices();
        InitializeSpeedValues();
        UpdateFillImage();
        InitializeButton();
    }

    private void InitializeServices()
    {
        _timeService = Services.GetService<ITimeService>();
        _vibrationService = Services.GetService<IVibrationService>();
    }

    private void InitializeButton()
    {
        _burstButton = GetComponent<Button>();
        _burstButton.onClick.AddListener(IncreaseSpeedStepByStep);
    }

    private void InitializeSpeedValues()
    {
        _defaultSpeed = _timeService.TimerSpeedFactor;
        _maxSpeed = _timeService.TimerSpeedFactorMax;
        _currentSpeed = _defaultSpeed;
    }

    private void IncreaseSpeedStepByStep()
    {
        ResetTweensAndScale();
        PlayTapAnimation();
        PlayHapticFeedback();
        UpdateSpeed();
        ScheduleSpeedReset();
    }

    private void ResetTweensAndScale()
    {
        _speedTween?.Kill();
        _tapAnimationTween?.Kill();
        transform.localScale = Vector3.one;
    }

    private void PlayTapAnimation()
    {
        _tapAnimationTween = transform.DOScale(1.15f, 0.16f)
            .SetUpdate(true)
            .SetEase(tapAnimationCurve);
    }

    private void PlayHapticFeedback()
    {
        _vibrationService.PlayHaptic(HapticTypes.SoftImpact);
    }

    private void UpdateSpeed()
    {
        _currentSpeed = Mathf.Min(_currentSpeed + 0.5f, _maxSpeed); 
        _timeService.SetTimerSpeedFactor(_currentSpeed);
        UpdateFillImage();
    }

    private void ScheduleSpeedReset()
    {
        _speedTween = DOVirtual.Float(_currentSpeed, _defaultSpeed, 1f, value =>
        {
            _currentSpeed = value;
            _timeService.SetTimerSpeedFactor(_currentSpeed);
            UpdateFillImage();
        }).SetDelay(0.2f);
    }

    private void UpdateFillImage()
    {
        fillImage.fillAmount = (_currentSpeed - _defaultSpeed) / (_maxSpeed - _defaultSpeed); 
    }
}
