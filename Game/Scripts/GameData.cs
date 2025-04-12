using System.Collections.Generic;

namespace CrankUp;

public class GameData {

    public int LevelProgress = 1;
    public Dictionary<int, int> LevelStars = new();
    public float Volume = 1f;
    public string Language = "en";
}