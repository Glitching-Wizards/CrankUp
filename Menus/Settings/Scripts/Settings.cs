using Godot;
using System;

namespace CrankUp
{
	public partial class Settings : Window
	{
		[Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";

		public override void _Ready()
		{

			TextureButton exitButton = GetNode<TextureButton>("ExitButton");
			if (exitButton == null)
			{
				GD.PrintErr("[ERROR] ExitButton not found in Settings.tscn");
			}
			else
			{
				exitButton.Pressed += ExitButtonPressed;
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public void BackButtonPressed()
		{
			this.Hide();
		}

		public void ExitButtonPressed()
		{
			GD.Print("Exit Pressed");

			Node currentScene = GetTree().CurrentScene;

			if (currentScene != null && currentScene.Name == "MainMenu"
			|| currentScene.Name == "Levels" || currentScene.Name == "Level1"
			|| currentScene.Name == "Level2" || currentScene.Name == "Level3"
			|| currentScene.Name == "Level4" || currentScene.Name == "Level5")
			{
				this.Hide();
			}
			else
			{
            	GetTree().ChangeSceneToFile(_levelsScenePath);
			}
		}
	}
}
