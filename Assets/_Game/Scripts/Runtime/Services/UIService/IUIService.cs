
public interface IUIService
{
    public void ShowPanel(Panel.Type panelType, bool isPopup = false, bool removePreviousPanels = false);
    public void AddToPreviousPanels(Panel.Type panelType, bool isPopup = false, bool removePreviousPanels = false);
    public void RemoveFromPreviousPanels(Panel.Type panelTypeToRemove);
    public Panel.Type ReturnToPreviousPanel();
    public void AddPanel(Panel.Type panelType, Panel panel);
}
