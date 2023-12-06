using UnityEngine;

public class MenuController : MonoBehaviour, IAnyLevelStatusListener
{
    [SerializeField] private GameObject failedPanel;
    [SerializeField] private GameObject completePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject shopPanel;

    private Contexts _contexts;
    private GameEntity _listener;
    private ITimeService _timeService;
    private ILevelService _levelService;

    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _levelService = Services.GetService<ILevelService>();
        _timeService = Services.GetService<ITimeService>();
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyLevelStatusListener(this);
    }

    public void OnAnyLevelStatus(GameEntity entity, LevelStatus status)
    {
        switch (status)
        {
            case LevelStatus.Continue:
                DeactivateAllPanels();
                break;
            case LevelStatus.Win:
                ActivatePanel(completePanel);
                break;
            case LevelStatus.Fail:
                ActivatePanel(failedPanel);
                break;
            case LevelStatus.Pause:
                ActivatePanel(pausePanel);
                break;
        }
    }

    private void DeactivateAllPanels()
    {
        foreach (var panel in GetAllPanels())
        {
            panel.SetActive(false);
        }
    }

    private GameObject[] GetAllPanels()
    {
        return new GameObject[] { failedPanel, completePanel, pausePanel, shopPanel };
    }

    private void ActivatePanel(GameObject panel)
    {
        DeactivateAllPanels();
        panel.SetActive(true);
    }

    public void NextLevel()
    {
        _contexts.game.ReplaceCurrentLevelIndex(_contexts.game.currentLevelIndex.Value + 1);
        RestartLevel();
    }

    public void TryAgain()
    {
        RestartLevel();
    }

    private void RestartLevel()
    {
        DeactivateAllPanels();
        _contexts.game.CreateEntity().isLoadLevel = true;
        _contexts.game.CreateEntity().isLevelRestart = true;
    }

    public void ResumeGame()
    {
        _levelService.SetLevelStatus(LevelStatus.Continue);
        _timeService.ResumeTime();
    }
    public void PauseGame()
    {
        _levelService.SetLevelStatus(LevelStatus.Pause);
        _timeService.PauseTime();
    }

    public void OpenShopPanel()
    {
        ActivatePanel(shopPanel);
    }
}