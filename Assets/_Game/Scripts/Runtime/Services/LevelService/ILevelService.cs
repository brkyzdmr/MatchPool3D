public interface ILevelService
{
    public bool IsLevelCompleted();
    public void SetLevelStatus(LevelStatus status);
    public void RefreshData();
}