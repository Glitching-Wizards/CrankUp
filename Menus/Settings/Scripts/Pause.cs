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
            RetryButton.Pressed += RetryButtonPressed;

            TextureButton MenuButton = GetNode<TextureButton>("Buttons/MenuButton");
            MenuButton.Pressed += MenuButtonPressed;

            TextureButton exitButton = GetNode<TextureButton>("Buttons/ExitButton");
            if (exitButton == null)
            {
                GD.PrintErr("[ERROR] ExitButton not found in Pause.tscn");
            }
            else
            {
                exitButton.Pressed += ExitButtonPressed;
            }

            TextureButton settingsButton = GetNode<TextureButton>("Buttons/SettingsButton");
            settingsButton.Pressed += SettingsButtonPressed;

            PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
            settingsWindow = (Window)settingsScene.Instantiate();
            AddChild(settingsWindow);
            settingsWindow.Hide();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        public void RetryButtonPressed()
        {
            GD.Print("Retry Pressed");

            GetTree().ReloadCurrentScene();
        }

        // Onko tarpeellinen?
        private void OnButtonPressed()
        {
            GD.Print("TOIMII");
            victoryScreen.Visible = true;
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
        }

        public void SettingsButtonPressed()
        {
            GD.Print("Settings Pressed");
            settingsWindow.Popup();
        }
    }
}

