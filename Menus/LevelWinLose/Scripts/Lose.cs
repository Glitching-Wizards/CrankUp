using Godot;
using System;

namespace CrankUp
{
	public partial class Lose : Window
	{
		[Export] private string _menuScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		[Export] private AudioStream clickSound;

		/// <summary>
		/// Calls for the buttonPressed methods when the buttons are pressed.
		/// </summary>
		public override void _Ready()
		{
			TextureButton retryButton = GetNode<TextureButton>("Buttons/RetryButton");
			retryButton.Pressed += RetryButtonPressed;

			TextureButton menuButton = GetNode<TextureButton>("Buttons/MenuButton");
			menuButton.Pressed += MenuButtonPressed;
		}

		/// <summary>
		/// Reloads the current scene when the retry button is pressed.
		/// </summary>
		public void RetryButtonPressed()
		{
			GD.Print("Retry Pressed");
			AudioManager.PlaySound(clickSound);
			GetTree().ReloadCurrentScene();
		}

		/// <summary>
		/// Changes the scene to the menu when the menu button is pressed and plays the click sound.
		/// </summary>
		public void MenuButtonPressed()
		{
			GD.Print("Menu Pressed");
			AudioManager.PlaySound(clickSound);
			GetTree().ChangeSceneToFile(_menuScenePath);
		}
	}
}