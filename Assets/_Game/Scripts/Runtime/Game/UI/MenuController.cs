using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuController : MonoBehaviour, IAnyLevelStatusListener
{
    [SerializeField] private List<Panel> panels;
    
    private Contexts _contexts;
    private GameEntity _listener;
    private IUIService _uiService;
    
    void Start()
    {
        _contexts = Contexts.sharedInstance;
        _uiService = Services.GetService<IUIService>();

        _listener = _contexts.game.CreateEntity();
        _listener.AddAnyLevelStatusListener(this);

        AddPanels();
        _uiService.ShowPanel(Panel.Type.Game);
    }

    private void AddPanels()
    {
        foreach (var panel in panels)
        {
            _uiService.AddPanel(panel.panelType, panel);
        }
    }

    public void OnAnyLevelStatus(GameEntity entity, LevelStatus status)
    {
        switch (status)
        {
            case LevelStatus.Continue:
                _uiService.ReturnToPreviousPanel();
                break;
            case LevelStatus.Win:
                _uiService.ShowPanel(Panel.Type.Win);
                break;
            case LevelStatus.Fail:
                _uiService.ShowPanel(Panel.Type.Fail);
                break;
            case LevelStatus.Pause:
                _uiService.ShowPanel(Panel.Type.Pause);
                break;
            case LevelStatus.Shop:
                _uiService.ShowPanel(Panel.Type.Shop);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }
    }
}