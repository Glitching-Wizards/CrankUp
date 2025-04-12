using Godot;
using System;

namespace CrankUp
{
    public partial class Levels : Node
    {
        [Export] private string _startLevel1ScenePath = "res://Menus/LevelStart/Scenes/StartLevel1.tscn";
        [Export] private string _startLevel2ScenePath = "res://Menus/LevelStart/Scenes/StartLevel2.tscn";
        [Export] private string _startLevel3ScenePath = "res://Menus/LevelStart/Scenes/StartLevel3.tscn";
        [Export] private string _startLevel4ScenePath = "res://Menus/LevelStart/Scenes/StartLevel4.tscn";
        [Export] private string _startLevel5ScenePath = "res://Menus/LevelStart/Scenes/StartLevel5.tscn";
        [Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
        private string _startLevelScenePath = "";
        private Window startLevelWindow;
        private Window settingsWindow;
        private TextureButton settingsButton;
        private const int TotalLevels = 5;


        public override void _Ready()
        {
            TextureButton LevelButton1 = GetNodeOrNull<TextureButton>("Buttons/Level1");
            LevelButton1.Pressed += () => LevelButtonPressed(1);

            TextureButton LevelButton2 = GetNodeOrNull<TextureButton>("Buttons/Level2");
            LevelButton2.Pressed += () => LevelButtonPressed(2);

            TextureButton LevelButton3 = GetNodeOrNull<TextureButton>("Buttons/Level3");
            LevelButton3.Pressed += () => LevelButtonPressed(3);

            TextureButton LevelButton4 = GetNodeOrNull<TextureButton>("Buttons/Level4");
            LevelButton4.Pressed += () => LevelButtonPressed(4);

            TextureButton LevelButton5 = GetNodeOrNull<TextureButton>("Buttons/Level5");
            LevelButton5.Pressed += () => LevelButtonPressed(5);

            TextureButton Back = GetNodeOrNull<TextureButton>("Buttons/BackButton");
            Back.Pressed += () => BackButtonPressed();

            settingsButton = GetNodeOrNull<TextureButton>("Buttons/SettingsButton");
            settingsButton.Pressed += SettingsButtonPressed;

            PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
            settingsWindow = (Window)settingsScene.Instantiate();
            AddChild(settingsWindow);
            settingsWindow.Hide();
        }

        public void BackButtonPressed()
        {
            GD.Print("Back Pressed");
            GetTree().ChangeSceneToFile("res://Menus/MainMenu/Scenes/MainMenu.tscn");
        }

        public void SettingsButtonPressed()
        {
            GD.Print("Settings Pressed");
            settingsWindow.Popup();
        }

        public void LevelButtonPressed(int level)
        {
            GD.Print($"Level {level} button pressed");

            _startLevelScenePath = GetLevelScenePath(level);

            PackedScene startSceneLevel = (PackedScene)GD.Load(_startLevelScenePath);
            startLevelWindow = (Window)startSceneLevel.Instantiate();
            AddChild(startLevelWindow);
            startLevelWindow.Popup();

            if (startLevelWindow is LevelStart levelStartWindow)
            {
                levelStartWindow.SetLevel(level);
            }
        }

        /// <summary>
        /// Valitun tason perusteella avataan oikea taso scene, kun pelaajaa painaa play.
        /// </summary>
        /// <param name="level">Valittu taso.</param>
        /// <returns></returns>
        private string GetLevelScenePath(int level)
        {
            return level switch
            {
                1 => _startLevel1ScenePath,
                2 => _startLevel2ScenePath,
                3 => _startLevel3ScenePath,
                4 => _startLevel4ScenePath,
                5 => _startLevel5ScenePath,
                _ => string.Empty
            };
        }

        public void SetupLevelButtons()
        {
            int unlocked = SaveSystem.GetGameData().LevelProgress;

            for (int i = 1; i <= TotalLevels; i++)
            {
                var button = GetNode<Button>($"LevelButtons/Level{i}");

                if (i <= unlocked)
                {
                    button.Disabled = false;
                    button.Text = $"Level {i} ({GetStars(i)}★)";
                }
                else
                {
                    button.Disabled = true;
                    button.Text = "Locked";
                }
            }
        }

        private int GetStars(int level)
        {
            return SaveSystem.GetGameData().LevelStars.TryGetValue(level, out int stars) ? stars : 0;
        }

    }
}