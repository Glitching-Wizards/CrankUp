using Godot;
using System;

namespace CrankUp
{
	public partial class MainMenu : Control
	{
		[Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
        private Window settingsWindow;
		private TextureButton settingsButton;

		private bool isSceneChanging = false;
		public override void _Ready()
		{
			Button playButton = GetNode<Button>("Buttons/PlayButton");
			playButton.Pressed += _on_play_button_pressed;

			Button creditButton = GetNode<Button>("Buttons/CreditsButton");
			creditButton.Pressed += CreditButtonPressed;

			settingsButton = GetNodeOrNull<TextureButton>("Buttons/SettingsButton");
            if (settingsButton == null)
            {
                GD.PrintErr("[ERROR] TextureButton 'Settings' not found in Levels!");
            }

            PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
            settingsWindow = (Window)settingsScene.Instantiate();
            AddChild(settingsWindow);
            settingsWindow.Visible = false;

            settingsButton.Pressed += SettingsButtonPressed;
		}

		public void _on_play_button_pressed()
		{
			if (isSceneChanging)
			{
				return;
			}

			GD.Print("Play Pressed");

			isSceneChanging = true;

            var tree = GetTree();
            if (tree != null)
            {
                tree.ChangeSceneToFile(_levelsScenePath);
                GD.Print("Play scene loaded");
            }
            else
            {
                GD.Print("Error: Tree is null");
				isSceneChanging = false;
            }
		}
		public void SettingsButtonPressed()
		{
			GD.Print("Settings Pressed");
            settingsWindow.Popup();
		}

		public void CreditButtonPressed()
		{
		}
	}
}

