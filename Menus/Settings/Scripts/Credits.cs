using Godot;
using System;

namespace CrankUp
{
    public partial class Credits : Window
    {
        [Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
        [Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
        private Window settingsWindow;
        private Window victoryScreen;
        public override void _Ready()
        {
            TextureButton exitButton = GetNodeOrNull<TextureButton>("Buttons/ExitButton");
            if (exitButton == null)
            {
                GD.PrintErr("[ERROR] ExitButton not found in PauseCredits.tscn");
            }
            else
            {
                exitButton.Pressed += ExitButtonPressed;
            }

            PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
            settingsWindow = (Window)settingsScene.Instantiate();
            AddChild(settingsWindow);
            settingsWindow.Hide();
        }

        public void ExitButtonPressed()
        {
            GD.Print("Exit Pressed");

            this.Hide();
        }
    }
}