using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PausePanel : Panel
{
    [Header("Pause Panel Settings")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button shopButton;

    private Tween _continueButtonTween;
    private ILevelService _levelService;

    private void Start()
    {
        _levelService = Services.GetService<ILevelService>();

        continueButton.onClick.AddListener(OnContinueButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        shopButton.onClick.AddListener(OnShopButtonClicked);
    }

    public void OnEnable()
    {
        ResetPausePanel();
        // _continueButtonTween = continueButton.transform.DOScale(1.1f, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnContinueButtonClicked()
    {
        // _continueButtonTween?.Kill();
        continueButton.interactable = false;
        
        _levelService.ResumeGame();
    }

    private void OnRestartButtonClicked()
    {
        // _continueButtonTween?.Kill();
        restartButton.interactable = false;
        
        _levelService.RestartGame();
    }

    private void OnShopButtonClicked()
    {
        _levelService.SetLevelStatus(LevelStatus.Shop);
    }

    private void ResetPausePanel()
    {
        continueButton.interactable = true;
        restartButton.interactable = true;
        // _continueButtonTween?.Kill();
        // continueButton.transform.localScale = Vector3.one;
    }
}