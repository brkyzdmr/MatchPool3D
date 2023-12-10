using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BurstButtonController : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private float _defaultSpeed;
    private float _maxSpeed;
    private float _currentSpeed;
    private Tween _speedTween;
    private Button _burstButton;
    private ITimeService _timeService;

    private void Start()
    {
        _timeService = Services.GetService<ITimeService>();
        _defaultSpeed = _timeService.TimerSpeedFactor;
        _maxSpeed = _timeService.TimerSpeedFactorMax;
        _currentSpeed = _defaultSpeed;
        
        _burstButton = GetComponent<Button>();
        _burstButton.onClick.AddListener(IncreaseSpeedStepByStep);
        UpdateFillImage();
    }

    private void IncreaseSpeedStepByStep()
    {
        _speedTween?.Kill();

        _currentSpeed = Mathf.Min(_currentSpeed + 0.5f, _maxSpeed); 
        _timeService.SetTimerSpeedFactor(_currentSpeed);
        UpdateFillImage();

        _speedTween = DOVirtual.Float(_currentSpeed, _defaultSpeed, 1f, value =>
        {
            _currentSpeed = value;
            _timeService.SetTimerSpeedFactor(_currentSpeed);
            UpdateFillImage();
        }).SetDelay(0.2f);
    }

    private void UpdateFillImage()
    {
        fillImage.fillAmount = (_currentSpeed - 1) / (_maxSpeed - 1); 
    }
}