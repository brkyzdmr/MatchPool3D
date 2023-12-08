using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopPanel : Panel
{
    [Header("Shop Panel Settings")]
    [SerializeField] private Button returnToMenuButton;

    private Tween _restartButtonTween;
    private ILevelService _levelService;
    private IUIService _uiService;

    private void Start()
    {
        _levelService = Services.GetService<ILevelService>();
        _uiService = Services.GetService<IUIService>();

        returnToMenuButton.onClick.AddListener(OnReturnToMenuButtonClicked);
    }

    private void OnEnable()
    {
        ResetShopPanel();
        // _restartButtonTween = returnToMenuButton.transform.DOScale(1.12f, 0.54f)
        //     .SetEase(Ease.InOutSine)
        //     .SetUpdate(true)
        //     .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnReturnToMenuButtonClicked()
    {
        // _restartButtonTween?.Kill();
        returnToMenuButton.interactable = false;

        _uiService.ReturnToPreviousPanel();
    }

    private void ResetShopPanel()
    {
        returnToMenuButton.interactable = true;
        // _restartButtonTween?.Kill();
        returnToMenuButton.transform.localScale = Vector3.one;
    }
}