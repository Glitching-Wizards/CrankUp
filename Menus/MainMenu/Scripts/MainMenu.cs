using Godot;
using System;

namespace CrankUp
{
	public partial class MainMenu : Control
	{
		[Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
		private Window settingsWindow;
		private Button settingsButton;

		private bool isSceneChanging = false;
		public override void _Ready()
		{
			Button playButton = GetNode<Button>("Buttons/PlayButton");
			playButton.Pressed += PlayButtonPressed;

			Button creditButton = GetNode<Button>("Buttons/CreditsButton");
			creditButton.Pressed += CreditButtonPressed;

			settingsButton = GetNodeOrNull<Button>("Buttons/SettingsButton");
			if (settingsButton == null)
			{
				GD.PrintErr("[ERROR] TextureButton 'Settings' not found in Levels!");
			}
			else
			{
				settingsButton.Pressed += SettingsButtonPressed;
			}

			PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
			if (settingsScene == null)
			{
				GD.PrintErr("[ERROR] Failed to load settings scene: " + _settingsScenePath);
				return;
			}
			settingsWindow = (Window)settingsScene.Instantiate();
			if (settingsWindow == null)
			{
				GD.PrintErr("[ERROR] Failed to instantiate settings window!");
				return;
			}

			AddChild(settingsWindow);
			settingsWindow.Visible = false;
		}

		public void PlayButtonPressed()
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

