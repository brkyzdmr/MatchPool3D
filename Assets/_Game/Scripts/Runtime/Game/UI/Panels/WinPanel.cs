using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WinPanel : Panel
{
    [Header("Win Panel Settings")]
    [SerializeField] private Button nextLevelButton;

    private Tween _nextLevelButtonTween;
    private ILevelService _levelService;

    private void Awake()
    {
        _levelService = Services.GetService<ILevelService>();

        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
    }

    private void OnEnable()
    {
        ResetWinPanel();
        _nextLevelButtonTween = nextLevelButton.transform.DOScale(1.12f, 0.54f)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnNextLevelButtonClicked()
    {
        _nextLevelButtonTween?.Kill();
        nextLevelButton.interactable = false;
        
        _levelService.NextLevel();
    }

    private void ResetWinPanel()
    {
        nextLevelButton.interactable = true;
        _nextLevelButtonTween?.Kill();
        nextLevelButton.transform.localScale = Vector3.one;
    }
}