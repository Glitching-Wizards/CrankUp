using Godot;
using System;

namespace CrankUp
{
    public partial class Pause : Window
    {
        /* Signal for tutorial start */
        [Signal]
        public delegate void TutorialStartedEventHandler();

        [Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
        [Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
        [Export] private AudioStream clickSound;
        private Window settingsWindow;
        private Window victoryScreen;

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

            TextureButton tutorialButton = GetNodeOrNull<TextureButton>("Buttons/TutorialButton");
            if (tutorialButton != null)
            {
                tutorialButton.Pressed += TutorialButtonPressed;
            }
            else
            {
                GD.PrintErr("[ERROR] tutorialButton not found!");
            }
        }


        public void RetryButtonPressed()
        {
            GD.Print("Retry Pressed");
            GetTree().Paused = false;
            AudioManager.PlaySound(clickSound);
            GetTree().ReloadCurrentScene();
        }

        public void MenuButtonPressed()
        {
            GD.Print("Menu Pressed");
            GetTree().Paused = false;
            AudioManager.PlaySound(clickSound);
            GetTree().ChangeSceneToFile(_levelsScenePath);
        }

        public void ExitButtonPressed()
        {
            GD.Print("Exit Pressed");
            GetTree().Paused = false;
            AudioManager.PlaySound(clickSound);
            this.Hide();

        }

        public void SettingsButtonPressed()
        {
            GD.Print("Settings Pressed");
            AudioManager.PlaySound(clickSound);
            settingsWindow.Popup();
        }

        public void TutorialButtonPressed()
        {
            GD.Print("Tutorial Pressed");
            AudioManager.PlaySound(clickSound);
            GetTree().Paused = false;
            EmitSignal("TutorialStartedEventHandler");
            this.Hide();
        }

        public bool IsPaused { get; private set; } = false;

        public void TogglePause()
        {
            IsPaused = !IsPaused;
            GetTree().Paused = IsPaused;
        }
    }
}

