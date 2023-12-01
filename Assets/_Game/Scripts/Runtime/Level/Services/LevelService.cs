using UnityEngine;

namespace Game
{
    public static class LevelService
    {
        public static int PlayerCurrentLevel
        {
            get => PlayerPrefs.GetInt(SaveGameService.PlayerCurrentLevelKey, 0);
            set => PlayerPrefs.SetInt(SaveGameService.PlayerCurrentLevelKey, value);
        }
    }

    public class SaveGameService
    {
        public const string PlayerCurrentLevelKey = "PlayerCurrentLevel";
    }
}
