using Godot;
using System;

namespace CrankUp
{
    public partial class Pause : Window
    {
        [Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
        [Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
        [Export] private AudioStream clickSound;
        private Window settingsWindow;
        private Window victoryScreen;


        /// <summary>
        /// Loads the windows and checks when the buttons are pressed and calls the methods for them.
        /// </summary>
        public override void _Ready()
        {
            TextureButton RetryButton = GetNodeOrNull<TextureButton>("Buttons/RetryButton");
            if (RetryButton != null)
            {
                RetryButton.Pressed += RetryButtonPressed;
            }
            else
            {
                GD.PrintErr("[ERROR] RetryButton not found! Check the node path.");
            }

            TextureButton MenuButton = GetNodeOrNull<TextureButton>("Buttons/MenuButton");
            if (MenuButton != null)
            {
                MenuButton.Pressed += MenuButtonPressed;
            }
            else
            {
                GD.PrintErr("[ERROR] MenuButton not found! Check the node path.");
            }

            TextureButton exitButton = GetNodeOrNull<TextureButton>("Buttons/ExitButton");
            exitButton.Pressed += ExitButtonPressed;

            TextureButton settingsButton = GetNodeOrNull<TextureButton>("Buttons/SettingsButton");
            if (settingsButton != null)
            {
                settingsButton.Pressed += SettingsButtonPressed;
            }
            else
            {
                GD.PrintErr("[ERROR] settingsButton not found!");
            }

            PackedScene settingsScene = GD.Load<PackedScene>(_settingsScenePath);
            settingsWindow = (Window)settingsScene.Instantiate();
            AddChild(settingsWindow);
            settingsWindow.Hide();
        }

        /// <summary>
        /// When the retry button is pressed, it reloads the current scene and plays the click sound.
        /// It also unpauses the game.
        /// </summary>
        public void RetryButtonPressed()
        {
            GD.Print("Retry Pressed");
            GetTree().Paused = false;
            AudioManager.PlaySound(clickSound);
            GetTree().ReloadCurrentScene();
        }

        /// <summary>
        /// When the menu button is pressed, it changes the scene to the levels scene and plays the click sound.
        /// It also unpauses the game.
        /// </summary>
        public void MenuButtonPressed()
        {
            GD.Print("Menu Pressed");
            GetTree().Paused = false;
            AudioManager.PlaySound(clickSound);
            GetTree().ChangeSceneToFile(_levelsScenePath);
        }

        /// <summary>
        /// When the exit button is pressed, it unpauses the game and hides the pause menu.
        /// It also plays the click sound.
        /// </summary>
        public void ExitButtonPressed()
        {
            GD.Print("Exit Pressed");
            GetTree().Paused = false;
            AudioManager.PlaySound(clickSound);
            this.Hide();
        }

        /// <summary>
        /// When the settings button is pressed, it opens the settings window and plays the click sound.
        /// It also unpauses the game.
        /// </summary>
        public void SettingsButtonPressed()
        {
            GD.Print("Settings Pressed");
            AudioManager.PlaySound(clickSound);
            settingsWindow.Popup();
        }

        /// <summary>
        /// When the pause menu is opened, it pauses the game.
        /// </summary>
        public bool IsPaused { get; private set; } = false;

        /// <summary>
        /// Toggles the pause state of the game.
        /// If the game is paused, it unpauses it and vice versa.
        /// </summary>
        public void TogglePause()
        {
            IsPaused = !IsPaused;
            GetTree().Paused = IsPaused;
        }
    }
}

