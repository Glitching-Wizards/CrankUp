using Godot;
using System;

namespace CrankUp
{
	public partial class WinLose : Window
	{
		[Export] private string _menuScenePath = "res://Menus/Levels/Scenes/Levels.tscn";

		public override void _Ready()
		{
			TextureButton retryButton = GetNode<TextureButton>("Buttons/RetryButton");
			retryButton.Pressed += RetryButtonPressed;

			TextureButton menuButton = GetNode<TextureButton>("Buttons/MenuButton");
			menuButton.Pressed += MenuButtonPressed;

			// next
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

		// next
	}
}