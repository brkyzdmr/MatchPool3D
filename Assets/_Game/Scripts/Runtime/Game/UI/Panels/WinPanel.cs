using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WinPanel : Panel, IAnyGoldRushEndListener
{
    [Header("Win Panel Settings")]
    [SerializeField] private Button nextLevelButton;

    private Tween _nextLevelButtonTween;
    private ILevelService _levelService;
    private Contexts _contexts;
    private GameEntity _listener;
    private Canvas _canvas;
    private int _canvasDefaultSortingOrder;

    private void Start()
    {
        _contexts = Contexts.sharedInstance;
        _levelService = Services.GetService<ILevelService>();
        
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyGoldRushEndListener(this);
        _canvas = GetComponent<Canvas>();
        _canvasDefaultSortingOrder = _canvas.sortingOrder;

        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        nextLevelButton.interactable = false;
    }

    private void OnEnable()
    {
        DOVirtual.DelayedCall(1.5f, () => EnableNextLevelButton());
    }

    private void OnDisable()
    {
        ResetWinPanel();
    }

    public void OnAnyGoldRushEnd(GameEntity entity)
    {
        EnableNextLevelButton();
    }
    

    private void OnNextLevelButtonClicked()
    {
        _nextLevelButtonTween?.Kill();
        nextLevelButton.interactable = false;
        
        _levelService.NextLevel();
    }

    private void ResetWinPanel()
    {
        _nextLevelButtonTween?.Kill();
        nextLevelButton.transform.localScale = Vector3.one;
        nextLevelButton.interactable = false;
        _canvas.sortingOrder = _canvasDefaultSortingOrder;
    }

    private void EnableNextLevelButton()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            _canvas.sortingOrder = 20;
            nextLevelButton.interactable = true;
            
            _nextLevelButtonTween = nextLevelButton.transform.DOScale(1.12f, 0.54f)
                .SetEase(Ease.InOutSine)
                .SetUpdate(true)
                .SetLoops(-1, LoopType.Yoyo);
        });
    }
}