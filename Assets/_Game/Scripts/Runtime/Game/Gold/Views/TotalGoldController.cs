using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using TMPro;
using UnityEngine;

public class TotalGoldController : MonoBehaviour, IAnyGoldEarnedListener, IAnyLevelEndListener, IAnyLevelReadyListener
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private AnimationCurve goldEarnedScaleCurve;

    private Contexts _contexts;
    private GameEntity _listener;
    private Tween _scaleTween;
    private IVibrationService _vibrationService;

    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _vibrationService = Services.GetService<IVibrationService>();
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyGoldEarnedListener(this);
        _listener.AddAnyLevelEndListener(this);
        _listener.AddAnyLevelReadyListener(this);
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
        _contexts.game.isGoldEarned = false;
    }

    public void OnAnyLevelEnd(GameEntity entity)
    {
        var remainingTimeSeconds = _contexts.game.remainingLevelTime.Value;
        if (remainingTimeSeconds > 0)
        {
            StartCoroutine(IncrementGoldAtLevelEnd(remainingTimeSeconds));
        }
    }

    private IEnumerator IncrementGoldAtLevelEnd(int remainingTimeSeconds)
    {
        var goldPerSecond = Services.GetService<IGameService>().GameConfig.GameConfig.goldPerLevelSecondsLeft;
        _contexts.game.isGoldRushStart = true;

        float decayFactor = Mathf.Pow(0.5f, 1.0f / (remainingTimeSeconds - 1));
        float currentDelay = 0.1f;

        for (int i = 0; i < remainingTimeSeconds; i++)
        {
            _vibrationService.PlayHaptic(HapticTypes.SoftImpact);
            yield return new WaitForSecondsRealtime(currentDelay);
            _contexts.game.ReplaceTotalGold(_contexts.game.totalGold.Value + goldPerSecond);
            _contexts.game.isGoldEarned = true;
            _contexts.game.ReplaceRemainingLevelTime(_contexts.game.remainingLevelTime.Value - 1);
            currentDelay *= decayFactor;
        }

        _contexts.game.isGoldRushEnd = true;
    }

    public void OnAnyLevelReady(GameEntity entity)
    {
        UpdateTotalGoldText();
        _contexts.game.isGoldRushStart = false;
        _contexts.game.isGoldRushEnd = false;
    }
}