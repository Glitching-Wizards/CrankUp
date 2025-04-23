using Godot;
using System;

namespace CrankUp
{
	public partial class Levels : Node
	{
		[Export] private string _startLevel1ScenePath = "res://Menus/LevelStart/Scenes/StartLevel1.tscn";
		[Export] private string _startLevel2ScenePath = "res://Menus/LevelStart/Scenes/StartLevel2.tscn";
		[Export] private string _startLevel3ScenePath = "res://Menus/LevelStart/Scenes/StartLevel3.tscn";
		[Export] private string _startLevel4ScenePath = "res://Menus/LevelStart/Scenes/StartLevel4.tscn";
		[Export] private string _startLevel5ScenePath = "res://Menus/LevelStart/Scenes/StartLevel5.tscn";
		[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
		[Export] private string _mainMenuScenePath = "res://Menus/MainMenu/Scenes/MainMenu.tscn";
		[Export] private AudioStream clickSound;
		[Export] private AudioStream menuMusic;
		private string _startLevelScenePath = "";
		private Window startLevelWindow;
		private Window settingsWindow;
		private TextureButton settingsButton;
		private const int TotalLevels = 5;

		/// <summary>
		/// This fuction sets up the level buttons and connects their signals to the appropriate functions.
		/// It also plays the menu music.
		/// </summary>
		public override void _Ready()
		{
			AudioManager.PlayMusic(menuMusic);

			TextureButton LevelButton1 = GetNodeOrNull<TextureButton>("Buttons/Level1");
			LevelButton1.Pressed += () => LevelButtonPressed(1);

			TextureButton LevelButton2 = GetNodeOrNull<TextureButton>("Buttons/Level2");
			LevelButton2.Pressed += () => LevelButtonPressed(2);

			TextureButton LevelButton3 = GetNodeOrNull<TextureButton>("Buttons/Level3");
			LevelButton3.Pressed += () => LevelButtonPressed(3);

			TextureButton LevelButton4 = GetNodeOrNull<TextureButton>("Buttons/Level4");
			LevelButton4.Pressed += () => LevelButtonPressed(4);

			TextureButton LevelButton5 = GetNodeOrNull<TextureButton>("Buttons/Level5");
			LevelButton5.Pressed += () => LevelButtonPressed(5);

			TextureButton Back = GetNodeOrNull<TextureButton>("Buttons/BackButton");
			Back.Pressed += () => BackButtonPressed();

			settingsButton = GetNodeOrNull<TextureButton>("Buttons/SettingsButton");
			settingsButton.Pressed += SettingsButtonPressed;

			PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
			settingsWindow = (Window)settingsScene.Instantiate();
			AddChild(settingsWindow);
			settingsWindow.Hide();
			SetupLevelButtons();
		}

		/// <summary>
		/// This function is called when the back button is pressed.
		/// It plays the click sound and opens the main menu.
		/// </summary>
		public void BackButtonPressed()
		{
			GD.Print("Back Pressed");
			AudioManager.PlaySound(clickSound);
			GetTree().ChangeSceneToFile(_mainMenuScenePath);
		}

		/// <summary>
		/// This function is called when the settings button is pressed.
		/// It plays th click sound and opens the settings window.
		/// </summary>
		public void SettingsButtonPressed()
		{
			GD.Print("Settings Pressed");
			AudioManager.PlaySound(clickSound);
			settingsWindow.Popup();
		}

		/// <summary>
		/// This function is called when a level button is pressed.
		/// It plays a sound and opens the corresponding level start window.
		/// </summary>
		/// <param name="level">The level number that was pressed.</param>
		public void LevelButtonPressed(int level)
		{
			GD.Print($"Level {level} button pressed");

			AudioManager.PlaySound(clickSound);

			_startLevelScenePath = GetLevelScenePath(level);

			PackedScene startSceneLevel = (PackedScene)GD.Load(_startLevelScenePath);
			startLevelWindow = (Window)startSceneLevel.Instantiate();

			if (startLevelWindow is LevelStart levelStartWindow)
			{
				levelStartWindow.SetLevel(level);
			}

			AddChild(startLevelWindow);
			startLevelWindow.Popup();
		}


		/// <summary>
		/// Based on the selected level, the correct level scene is opened when the player presses play.
		/// </summary>
		/// <param name="level">Selected level.</param>
		/// <returns></returns>
		private string GetLevelScenePath(int level)
		{
			return level switch
			{
				1 => _startLevel1ScenePath,
				2 => _startLevel2ScenePath,
				3 => _startLevel3ScenePath,
				4 => _startLevel4ScenePath,
				5 => _startLevel5ScenePath,
				_ => string.Empty
			};
		}

		/// <summary>
		/// This function sets up the level buttons based on the player's progress.
		/// It enables or disables the buttons and sets the text to show the number of stars earned.
		/// </summary>
		public void SetupLevelButtons()
		{
			int unlocked = SaveSystem.GetGameData().LevelProgress;

			for (int i = 1; i <= TotalLevels; i++)
			{
				var button = GetNode<Button>($"LevelButtons/Level{i}");

				if (i <= unlocked)
				{
					button.Disabled = false;
					button.Text = $"Level {i} ({GetStars(i)}★)";
				}
				else
				{
					button.Disabled = true;
					button.Text = "Locked";
				}
			}
		}

		/// <summary>
		/// This function gets the number of stars earned for a specific level.
		/// It checks the game data to see if the level has been completed and how many stars were earned.
		/// </summary>
		/// <param name="level">The level number.</param>
		/// <returns>The number of stars earned for the level.</returns>
		private int GetStars(int level)
		{
			return SaveSystem.GetGameData().LevelStars.TryGetValue(level, out int stars) ? stars : 0;
		}

	}
}
