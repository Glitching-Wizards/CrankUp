using Godot;
using System;

namespace CrankUp
{
	public partial class WinLose : Window
	{
		[Export] private string _menuScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		[Export] private string _startLevel2ScenePath = "res://Menus/LevelStart/Scenes/StartLevel2.tscn";
		[Export] private string _startLevel3ScenePath = "res://Menus/LevelStart/Scenes/StartLevel3.tscn";
		[Export] private string _startLevel4ScenePath = "res://Menus/LevelStart/Scenes/StartLevel4.tscn";
		[Export] private string _startLevel5ScenePath = "res://Menus/LevelStart/Scenes/StartLevel5.tscn";

		private string _startLevelScenePath = "";
		private Window startLevelWindow;


		public override void _Ready()
		{
			TextureButton retryButton = GetNode<TextureButton>("Buttons/RetryButton");
			retryButton.Pressed += RetryButtonPressed;

			TextureButton menuButton = GetNode<TextureButton>("Buttons/MenuButton");
			menuButton.Pressed += MenuButtonPressed;

			TextureButton nextButton = GetNode<TextureButton>("Buttons/NextButton");
			nextButton.Pressed += NextButtonPressed;
		}

		public void RetryButtonPressed()
		{
			GD.Print("Retry Pressed");
			GetTree().ReloadCurrentScene();
		}

		public void MenuButtonPressed()
		{
			GetTree().ChangeSceneToFile(_menuScenePath);
		}

		public void NextButtonPressed()
		{
			GD.Print("Next Pressed");
			Node currentScene = GetTree().CurrentScene;

			if (currentScene == null)
			{
				GD.PrintErr("[ERROR] No active scene found!");
				return;
			}

			int nextLevel = 0;

			switch (currentScene.Name)
			{
				case "Level1":
					nextLevel = 2;
					break;

				case "Level2":
					nextLevel = 3;
					break;

				case "Level3":
					nextLevel = 4;
					break;

				case "Level4":
					nextLevel = 5;
					break;

				default:
					GD.PrintErr("[ERROR] No active level found!");
					return;
			}
			
			_startLevelScenePath = GetLevelScenePath(nextLevel);

			PackedScene startSceneLevel = (PackedScene)GD.Load(_startLevelScenePath);
			startLevelWindow = (Window)startSceneLevel.Instantiate();
			AddChild(startLevelWindow);
			startLevelWindow.Popup();

			if (startLevelWindow is LevelStart levelStartWindow)
			{
				levelStartWindow.SetLevel(nextLevel);
			}

		}

		private string GetLevelScenePath(int nextLevel)
		{
			return nextLevel switch
			{
				2 => _startLevel2ScenePath,
				3 => _startLevel3ScenePath,
				4 => _startLevel4ScenePath,
				5 => _startLevel5ScenePath,
				_ => string.Empty
			};
		}
	}
}
