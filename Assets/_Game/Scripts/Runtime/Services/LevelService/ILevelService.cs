public interface ILevelService
{
    public bool IsLevelCompleted();
    public void SetLevelStatus(LevelStatus status);
    public void RefreshData();

    public void ResumeGame();
    public void PauseGame();
    public void RestartGame();

    public void NextLevel();
}