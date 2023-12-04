using TMPro;
using UnityEngine;

public class LevelNameController : MonoBehaviour, IAnyLevelReadyListener
{
    [SerializeField] private TMP_Text label;

    private Contexts _contexts;
    private GameEntity _listener;
    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyLevelReadyListener(this);
    }
    public void OnAnyLevelReady(GameEntity entity)
    {
        if (!_contexts.game.isLevelReady)
            return;
        
        var levelConfig = LevelService.LevelsConfig;
        var level = LevelService.CurrentLevel;
        var name = levelConfig.Levels.levels[level].name;
        
        label.text = $"{name}";
    }
}
