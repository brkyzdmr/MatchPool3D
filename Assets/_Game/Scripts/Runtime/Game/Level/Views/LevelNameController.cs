using TMPro;
using UnityEngine;

public class LevelNameController : MonoBehaviour, IAnyLevelReadyListener
{
    [SerializeField] private TMP_Text label;

    private Contexts _contexts;
    private GameEntity _listener;
    private ILevelService _levelService;

    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _levelService = Services.GetService<ILevelService>();
        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyLevelReadyListener(this);
    }
    public void OnAnyLevelReady(GameEntity entity)
    {
        if (!_contexts.game.isLevelReady)
            return;

        var levelName = _contexts.game.levelName.Value;
        label.text = $"{levelName}";
    }
}
