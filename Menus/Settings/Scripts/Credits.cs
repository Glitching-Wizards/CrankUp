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
        [Export] private AudioStream clickSound;
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

<<<<<<< HEAD
<<<<<<< HEAD
=======
            AudioManager.PlaySound(clickSound);
>>>>>>> 407834f9439e6171bd94341ac98b85bd1cbe1b64
=======
>>>>>>> 8a165564561152121a3ce3d167ff9dc1e441c839
            this.Hide();
        }
    }
}