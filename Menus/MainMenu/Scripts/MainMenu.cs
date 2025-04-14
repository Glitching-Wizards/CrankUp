using Godot;
using System;

namespace CrankUp
{
	public partial class MainMenu : Control
	{
		[Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
		[Export] private string _creditsScenePath = "res://Menus/Settings/Scenes/Credits.tscn";
		[Export] private AudioStream clickSound;
		[Export] private AudioStream menuMusic;
		private Window settingsWindow;
		private Window creditsWindow;

		private bool isSceneChanging = false;
		public override void _Ready()
		{
			SaveSystem.LoadGame();
<<<<<<< HEAD
			AudioManager.PlayMusic(menuMusic);
=======

			AudioManager.PlayMusic(menuMusic);

>>>>>>> 407834f9439e6171bd94341ac98b85bd1cbe1b64
			Button playButton = GetNodeOrNull<Button>("Buttons/PlayButton");
			playButton.Pressed += PlayButtonPressed;

			Button settingsButton = GetNodeOrNull<Button>("Buttons/SettingsButton");
			settingsButton.Pressed += SettingsButtonPressed;

			PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
			settingsWindow = (Window)settingsScene.Instantiate();
			AddChild(settingsWindow);
			settingsWindow.Hide();

			Button creditsButton = GetNodeOrNull<Button>("Buttons/CreditsButton");
			creditsButton.Pressed += CreditsButtonPressed;

			PackedScene creditsScene = (PackedScene)GD.Load(_creditsScenePath);
			creditsWindow = (Window)creditsScene.Instantiate();
			AddChild(creditsWindow);
			creditsWindow.Hide();
		}

		public void PlayButtonPressed()
		{
			if (isSceneChanging)
			{
				return;
			}

			GD.Print("Play Pressed");

			AudioManager.PlaySound(clickSound);

			isSceneChanging = true;

			var tree = GetTree();
			if (tree != null)
			{
				tree.ChangeSceneToFile(_levelsScenePath);
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
			AudioManager.PlaySound(clickSound);
			settingsWindow.Popup();
		}

		public void CreditsButtonPressed()
		{
			GD.Print("Credits Pressed");
			AudioManager.PlaySound(clickSound);
			creditsWindow.Popup();

		}
	}
}
