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

		/// <summary>
		/// Checks when the buttons are pressed and calls the methods for them.
		/// </summary>
		public override void _Ready()
		{
			SaveSystem.LoadGame();
			AudioManager.PlayMusic(menuMusic);
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

		/// <summary>
		/// When play button is pressed it changes the scene to the level start scene and plays the click sound.
		/// </summary>
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

		/// <summary>
		/// When play button is pressed it opens the settings window and plays the click sound.
		/// </summary>
		public void SettingsButtonPressed()
		{
			GD.Print("Settings Pressed");
			AudioManager.PlaySound(clickSound);
			settingsWindow.Popup();
		}

		/// <summary>
		/// When play button is pressed it opens the credits window and plays the click sound.
		/// </summary>
		public void CreditsButtonPressed()
		{
			GD.Print("Credits Pressed");
			AudioManager.PlaySound(clickSound);
			creditsWindow.Popup();
		}

		/// <summary>
		/// Called when the app is about to quit (also works on mobile app close).
		/// Ensures that game progress is saved.
		/// </summary>
		public override void _Notification(int what)
		{
			if (what == NotificationExitTree)
			{
				GD.Print("[MainMenu] App is exiting. Saving game...");
				SaveSystem.SaveGame();
			}
		}

	}
}
