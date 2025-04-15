using Godot;
using System;
using System.IO;
using System.Text.Json;
using CrankUp;

namespace CrankUp
{
    public static class SaveSystem
    {
        private static readonly string SavePath = "user://savegame.json";

        private static GameData data = LoadGame();

        public static void OnLevelCompleted(int levelNumber, int stars)
        {
            if (!data.LevelStars.ContainsKey(levelNumber) || stars > data.LevelStars[levelNumber])
                data.LevelStars[levelNumber] = stars;

            if (levelNumber >= data.LevelProgress)
                data.LevelProgress = levelNumber + 1;

            SaveGame();
        }

        public static GameData GetGameData() => data;

        public static void SaveGame()
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ProjectSettings.GlobalizePath(SavePath), json);
        }

        public static GameData LoadGame()
        {
            string path = ProjectSettings.GlobalizePath(SavePath);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<GameData>(json) ?? new GameData();
            }

            return new GameData();
        }
    }
}
