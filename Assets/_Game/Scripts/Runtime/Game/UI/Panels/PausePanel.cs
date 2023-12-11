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
    }

    private void OnContinueButtonClicked()
    {
        continueButton.interactable = false;
        
        _levelService.ResumeGame();
    }

    private void OnRestartButtonClicked()
    {
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
    }
}