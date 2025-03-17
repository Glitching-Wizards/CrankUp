using Godot;
using System;

namespace CrankUp
{
	public partial class Settings : Window
	{
		public override void _Ready()
		{
			TextureButton backButton = GetNode<TextureButton>("BackButton");
			backButton.Pressed += () => BackButtonPressed();

			Button quitButton = GetNode<Button>("QuitButton");
			quitButton.Pressed += QuitButtonPressed;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public void BackButtonPressed()
		{
			this.Hide();
		}

		public void QuitButtonPressed()
		{
			GD.Print("Quit Pressed");

			Node currentScene = GetTree().CurrentScene;

			if (currentScene != null && currentScene.Name == "MainMenu" || currentScene.Name == "Levels")
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
