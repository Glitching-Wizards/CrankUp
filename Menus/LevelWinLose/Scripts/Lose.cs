using Godot;
using System;

namespace CrankUp
{
	public partial class Lose : Window
	{
		[Export] private string _menuScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		[Export] private AudioStream clickSound;

		public override void _Ready()
		{
			TextureButton retryButton = GetNode<TextureButton>("Buttons/RetryButton");
			retryButton.Pressed += RetryButtonPressed;

			TextureButton menuButton = GetNode<TextureButton>("Buttons/MenuButton");
			menuButton.Pressed += MenuButtonPressed;
		}

		public void RetryButtonPressed()
		{
			GD.Print("Retry Pressed");
			AudioManager.PlaySound(clickSound);
			GetTree().ReloadCurrentScene();
		}

		public void MenuButtonPressed()
		{
			GD.Print("Menu Pressed");
			AudioManager.PlaySound(clickSound);
			GetTree().ChangeSceneToFile(_menuScenePath);
		}
	}
}