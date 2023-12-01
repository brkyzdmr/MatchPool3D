using UnityEngine;

public class MenuController : MonoBehaviour, IAnyLevelStatusListener
{
    [SerializeField] private GameObject failedPanel;
    [SerializeField] private GameObject completePanel;

    private Contexts _contexts;
    private GameEntity _listener;

    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyLevelStatusListener(this);
    }

    public void OnAnyLevelStatus(GameEntity entity, LevelStatus status)
    {
        switch (status)
        {
            case LevelStatus.Continue:
                DeactivatePanels();
                break;
            case LevelStatus.Win:
                ShowCompletePanel();
                break;
            case LevelStatus.Fail:
                ShowFailedPanel();
                break;
        }
    }
    
    private void DeactivatePanels()
    {
        failedPanel.SetActive(false);
        completePanel.SetActive(false);
    }

    private void ShowCompletePanel()
    {
        failedPanel.SetActive(false);
        completePanel.SetActive(true);
    }

    private void ShowFailedPanel()
    {
        failedPanel.SetActive(true);
        completePanel.SetActive(false);
    }

    public void NextLevel()
    {
        LevelService.PlayerCurrentLevel++;
        Load();
    }

    public void TryAgain()
    {
        Load();
    }

    private void Load()
    {
        failedPanel.SetActive(false);
        completePanel.SetActive(false);
        _contexts.game.CreateEntity().isLoadLevel = true;
    }
}
