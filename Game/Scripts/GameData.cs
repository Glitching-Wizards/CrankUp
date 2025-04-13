using System.Collections.Generic;

namespace CrankUp;

    public class GameData{
        public int LevelProgress = 1;
        public Dictionary<int, int> LevelStars = new();

        public float MasterVolume = -6f;
        public float MusicVolume = -6f;
        public float SfxVolume = -6f;
        public string Language = "en";

        public static GameData CreateDefaults() {
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
