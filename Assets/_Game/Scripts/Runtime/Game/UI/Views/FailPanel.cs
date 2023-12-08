using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FailPanel : Panel
{
    [Header("Fail Panel Settings")]
    [SerializeField] private Button restartButton;

    private Tween _restartButtonTween;
    private ILevelService _levelService;

    private void Start()
    {
        _levelService = Services.GetService<ILevelService>();
        
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnEnable()
    {
        ResetFailedPanel();
        _restartButtonTween = restartButton.transform.DOScale(1.12f, 0.54f)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnRestartButtonClicked()
    {
        _restartButtonTween?.Kill();
        restartButton.interactable = false;
        
        _levelService.RestartGame();
    }

    private void ResetFailedPanel()
    {
        restartButton.interactable = true;
        _restartButtonTween?.Kill();
        restartButton.transform.localScale = Vector3.one;
    }
}
