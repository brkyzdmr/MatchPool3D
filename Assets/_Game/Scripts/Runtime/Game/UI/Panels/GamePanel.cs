using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GamePanel : Panel
{
    [Header("Game Panel Settings")]
    [SerializeField] private Button pauseButton;

    private Tween _pauseButtonTween;
    private ILevelService _levelService;
    
    private void Start()
    {
        _levelService = Services.GetService<ILevelService>();

        pauseButton.onClick.AddListener(OnPauseButtonClicked);
    }

    private void OnEnable()
    {
        ResetWinPanel();
        // _pauseButtonTween = pauseButton.transform.DOScale(1.1f, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnPauseButtonClicked()
    {
        _pauseButtonTween?.Kill();
        // pauseButton.interactable = false;
        
        _levelService.PauseGame();
    }

    private void ResetWinPanel()
    {
        // pauseButton.interactable = true;
        // _pauseButtonTween?.Kill();
        pauseButton.transform.localScale = Vector3.one;
    }
}