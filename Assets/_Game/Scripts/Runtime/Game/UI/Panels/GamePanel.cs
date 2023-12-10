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
    }

    private void OnPauseButtonClicked()
    {
        _pauseButtonTween?.Kill();
        _levelService.PauseGame();
    }

    private void ResetWinPanel()
    {
        pauseButton.transform.localScale = Vector3.one;
    }
}