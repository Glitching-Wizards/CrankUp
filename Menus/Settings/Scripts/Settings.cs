using Godot;
using System;

namespace CrankUp
{
	public partial class Settings : Window
	{
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

			if (currentScene != null && currentScene.Name == "MainMenu" || currentScene.Name == "Levels" || currentScene.Name == "Level1")
			{
				this.Hide();
			}
			else
			{
            	GetTree().ChangeSceneToFile("res://Menus/Levels/Scenes/Levels.tscn");
			}
		}
	}
}
