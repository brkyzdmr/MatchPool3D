
using System;
using System.Collections.Generic;

public class UIService : Service, IUIService
{
    private readonly Contexts _contexts;
    private readonly Dictionary<Panel.Type, Panel> _panels;
    private Stack<Panel> _previousPanels = new Stack<Panel>();
    private Panel _topPanelOrPopup;
    private Panel _currentPanel;
    private Panel _lastShownPanel;
    public Panel.Type GetCurrentPanelType() => _currentPanel?.panelType ?? Panel.Type.None;
    public Panel.Type GetTopPanelOrPopupType() => _topPanelOrPopup?.panelType ?? Panel.Type.None;
    public Panel.Type GetLastPanelType() => _lastShownPanel?.panelType ?? Panel.Type.None;

    public UIService(Contexts contexts) : base(contexts)
    {
        _contexts = contexts;
        _panels = new Dictionary<Panel.Type, Panel>();
    }

    public void ShowPanel(Panel.Type panelType, bool isPopup = false, bool removePreviousPanels = false)
    {
        if (removePreviousPanels) { _previousPanels.Clear(); }

        Panel panelToShow = GetPanel(panelType);
        panelToShow.isCurrentlyPopup = panelToShow.isAlwaysPopup || isPopup;

        HandleCurrentPanel(panelToShow);
        HandleTopPanel(panelToShow, removePreviousPanels);
        ActivatePanel(panelToShow);
    }

    public void AddToPreviousPanels(Panel.Type panelType, bool isPopup = false, bool removePreviousPanels = false)
    {
        if (removePreviousPanels) { _previousPanels.Clear(); }

        Panel panelToAdd = GetPanel(panelType);
        panelToAdd.isCurrentlyPopup = panelToAdd.isAlwaysPopup || isPopup;
        _previousPanels.Push(panelToAdd);
    }

    public void RemoveFromPreviousPanels(Panel.Type panelTypeToRemove)
    {
        var tempStack = new Stack<Panel>();
        while (_previousPanels.TryPop(out var panel))
        {
            if (panel.panelType != panelTypeToRemove)
                tempStack.Push(panel);
        }

        _previousPanels = tempStack;
    }

    public Panel.Type ReturnToPreviousPanel()
    {
        if (_previousPanels.Count == 0) { return Panel.Type.None; }

        DeactivatePanel(_topPanelOrPopup);
        _lastShownPanel = _topPanelOrPopup;
        _topPanelOrPopup = _previousPanels.Pop();
        ActivatePanel(_topPanelOrPopup);
        _currentPanel = _topPanelOrPopup.isCurrentlyPopup ? _currentPanel : _topPanelOrPopup;

        return _topPanelOrPopup.panelType;
    }

    private Panel GetPanel(Panel.Type panelType) => _panels.TryGetValue(panelType, out var panel) ? panel : null;
    
    public void AddPanel(Panel.Type panelType, Panel panel)
    {
        if(panel == null) throw new ArgumentNullException(nameof(panel));

        _panels[panelType] = panel;
    }

    private void ActivatePanel(Panel panel)
    {
        panel.gameObject.SetActive(true);
        panel.isCurrentlyOpen = true;

        if (panel.filterImage == null) return;
        panel.filterImage.SetActive(true);
    }

    private void DeactivatePanel(Panel panel)
    {
        panel.gameObject.SetActive(false);
        panel.isCurrentlyOpen = false;
        
        if (panel.filterImage == null) return;
        panel.filterImage.SetActive(false);
    }

    private void HandleCurrentPanel(Panel panelToShow)
    {
        if (_currentPanel == null || !panelToShow.isCurrentlyPopup) { _currentPanel = panelToShow; }    
    }

    private void HandleTopPanel(Panel panelToShow, bool removePreviousPanels)
    {
        if (_topPanelOrPopup != null) 
        {
            HandlePanelVisibility(_topPanelOrPopup, panelToShow, removePreviousPanels);
        }
        _lastShownPanel = _topPanelOrPopup;
        _topPanelOrPopup = panelToShow;
    }

    private void HandlePanelVisibility(Panel previousPanel, Panel newPanel, bool removePreviousPanels)
    {
        if (previousPanel.isCurrentlyPopup || !newPanel.isCurrentlyPopup) { DeactivatePanel(previousPanel); }
        if (!removePreviousPanels) { _previousPanels.Push(previousPanel); }    
    }
}
