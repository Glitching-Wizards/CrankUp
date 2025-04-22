using System.Collections.Generic;

namespace CrankUp
{
    public class GameData
    {
        public int LevelProgress { get; set; } = 1;
        public Dictionary<int, int> LevelStars { get; set; } = new();

        public float MasterVolume { get; set; } = -6f;
        public float MusicVolume { get; set; } = -6f;
        public float SfxVolume { get; set; } = -6f;
        public string Language { get; set; } = "en";

        public static GameData CreateDefaults()
        {
            return new GameData
            {
                Language = "en",
                MasterVolume = -6f,
                MusicVolume = -6f,
                SfxVolume = -6f,
                LevelProgress = 1,
                LevelStars = new Dictionary<int, int>()
            };
        }
    }
}
