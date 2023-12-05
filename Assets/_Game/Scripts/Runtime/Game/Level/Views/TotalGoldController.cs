using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TotalGoldController : MonoBehaviour, IAnyLoadListener, IAnyGoldEarnedListener, IAnyLevelEndListener
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private AnimationCurve goldEarnedScaleCurve;

    private Contexts _contexts;
    private GameEntity _listener;
    private Tween _scaleTween;

    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyGoldEarnedListener(this);
        _listener.AddAnyLoadListener(this);
        _listener.AddAnyLevelEndListener(this);
        UpdateTotalGoldText();
    }

    public void OnAnyLoad(GameEntity entity)
    {
        UpdateTotalGoldText();
    }

    public void OnAnyGoldEarned(GameEntity entity)
    {
        _scaleTween?.Kill();
        label.transform.localScale = Vector3.one;
        _scaleTween = label.DOScale(1.3f, 0.2f)
            .SetEase(goldEarnedScaleCurve)
            .SetUpdate(true)
            .OnComplete(() => UpdateTotalGoldText());
    }

    private void UpdateTotalGoldText()
    {
        var totalGold = _contexts.game.totalGold.Value;
        label.text = $"{totalGold}";
        Debug.Log("Total Gold: " + totalGold);
        _contexts.game.isGoldEarned = false;
    }

    public void OnAnyLevelEnd(GameEntity entity)
    {
        StartCoroutine(IncrementGoldAtLevelEnd());
    }

    private IEnumerator IncrementGoldAtLevelEnd()
    {
        Debug.Log("Level End: Coin Rush!");
        var remainingTimeSeconds = _contexts.game.remainingLevelTime.Value;
        var goldPerSecond = Services.GetService<IGameService>().GameConfig.GameConfig.goldPerLevelSecondsLeft;

        for (int i = 0; i < remainingTimeSeconds; i++)
        {
            yield return new WaitForSecondsRealtime(0.05f * (remainingTimeSeconds - i));
            _contexts.game.ReplaceTotalGold(_contexts.game.totalGold.Value + goldPerSecond);
            _contexts.game.isGoldEarned = true;
            _contexts.game.ReplaceRemainingLevelTime(_contexts.game.remainingLevelTime.Value - 1);
        }
    }
}