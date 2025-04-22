using System;
using System.IO;
using System.Text.Json;
using Godot;

namespace CrankUp
{
    public static class SaveSystem
    {
        private static string SavePath => "user://savegame.json";
        private static GameData _gameData;

        public static GameData GetGameData()
        {
            return _gameData;
        }

        public static void LoadGame()
        {
            string path = ProjectSettings.GlobalizePath(SavePath);
            GD.Print("[SaveSystem] LoadGame() called");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                GD.Print("[SaveSystem] Loaded JSON: " + json);

                _gameData = JsonSerializer.Deserialize<GameData>(json);

                foreach (var kvp in _gameData.LevelStars)
                {
                    GD.Print($"[SaveSystem] Loaded Stars: Level {kvp.Key} => {kvp.Value}");
                }
            }
            else
            {
                GD.Print("[SaveSystem] Save file not found. Creating defaults.");
                _gameData = GameData.CreateDefaults();
                SaveGame();
            }
        }

        public static void SaveGame()
        {
            GD.Print("[SaveSystem] SaveGame() called");
            GD.Print("[SaveSystem] LevelProgress: " + _gameData.LevelProgress);

            foreach (var kvp in _gameData.LevelStars)
            {
                GD.Print($"[SaveSystem] Saving Stars: Level {kvp.Key} => {kvp.Value}");
            }

            string json = JsonSerializer.Serialize(_gameData, new JsonSerializerOptions { WriteIndented = true });

            string path = ProjectSettings.GlobalizePath(SavePath);
            GD.Print("[SaveSystem] Writing save to: " + path);
            File.WriteAllText(path, json);
        }

        public static void OnLevelCompleted(int level, int stars)
        {
            if (level > _gameData.LevelProgress)
                _gameData.LevelProgress = level;

            if (_gameData.LevelStars.TryGetValue(level, out int existingStars))
            {
                if (stars > existingStars)
                    _gameData.LevelStars[level] = stars;
            }
            else
            {
                _gameData.LevelStars[level] = stars;
            }
            
            GD.Print($"[SaveSystem] OnLevelCompleted: Level {level}, Stars Earned: {stars}");
            GD.Print("[SaveSystem] Current Stars Before Save:");
            foreach (var kvp in _gameData.LevelStars)
            {
                GD.Print($"  Level {kvp.Key}: {kvp.Value}");
            }

            SaveGame();
        }
    }
}