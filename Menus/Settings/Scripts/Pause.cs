using Godot;
using System;

namespace CrankUp
{
    public partial class Pause : Window
    {
        [Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
        [Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
        private Window settingsWindow;
        private Window victoryScreen;
        public override void _Ready()
        {
            TextureButton RetryButton = GetNode<TextureButton>("Buttons/RetryButton");
            if (RetryButton != null)
            {
                RetryButton.Pressed += RetryButtonPressed;
            }
            else
            {
                GD.PrintErr("[ERROR] RetryButton not found! Check the node path.");
            }

            TextureButton MenuButton = GetNode<TextureButton>("Buttons/MenuButton");
            MenuButton.Pressed += MenuButtonPressed;

            TextureButton exitButton = GetNode<TextureButton>("Buttons/ExitButton");
            exitButton.Pressed += ExitButtonPressed;

            TextureButton settingsButton = GetNode<TextureButton>("Buttons/SettingsButton");
            settingsButton.Pressed += SettingsButtonPressed;

            PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
            settingsWindow = (Window)settingsScene.Instantiate();
            AddChild(settingsWindow);
            settingsWindow.Hide();

            TextureButton tutorialButton = GetNode<TextureButton>("Buttons/TutorialButton");
            tutorialButton.Pressed += TutorialButtonPressed;
        }


        public void RetryButtonPressed()
        {
            GD.Print("Retry Pressed");
            GetTree().ReloadCurrentScene();
        }

        public void MenuButtonPressed()
        {
            GD.Print("Menu Pressed");
            GetTree().ChangeSceneToFile(_levelsScenePath);
        }

        public void ExitButtonPressed()
        {
            GD.Print("Exit Pressed");

            Node currentScene = GetTree().CurrentScene;

            this.Hide();
            GetTree().Paused = false;

        }

        public void SettingsButtonPressed()
        {
            GD.Print("Settings Pressed");
            settingsWindow.Popup();
        }

        public void TutorialButtonPressed()
        {
            GD.Print("Tutorial Pressed");

            // open tutorial
            Node tutorialNode = GetTree().Root.FindChild("Tutorial", true, false);
            if (tutorialNode == null)
            {
                GD.Print("[INFO] Tutorial is not found. Instantiating it...");
                PackedScene tutorialScene = (PackedScene)GD.Load("res:/");
                tutorialNode = tutorialScene.Instantiate();
                GetTree().Root.AddChild(tutorialNode);
            }

            if (tutorialNode is Tutorial tutorial)
            {
                tutorial.StartTutorial();
            }
            else
            {
                GD.PrintErr("[ERROR] Failed to cast Tutorial node.");
            }
        }

        public bool IsPaused { get; private set; } = false;

        public void TogglePause()
        {
            IsPaused = !IsPaused;
            GetTree().Paused = IsPaused;
        }
    }
}

