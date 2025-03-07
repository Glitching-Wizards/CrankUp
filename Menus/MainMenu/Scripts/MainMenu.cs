using Godot;
using System;

namespace CrankUp
{
	public partial class MainMenu : Control
	{
		[Export] private string _level1ScenePath = "res://Game/Level1/Scenes/Level1.tscn";
		[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
		private Window settingsWindow;
		private bool isSceneChanging = false;
		public override void _Ready()
		{
			PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
    		settingsWindow = (Window)settingsScene.Instantiate();
    		AddChild(settingsWindow);
    		settingsWindow.Visible = false;

			TextureButton playButton = GetNode<TextureButton>("Buttons/PlayButton");
			playButton.Pressed += _on_play_button_pressed;

			TextureButton settingsButton = GetNode<TextureButton>("Buttons/SettingsButton");
			settingsButton.Pressed += _on_settings_button_pressed;

			TextureButton quitButton = GetNode<TextureButton>("Buttons/QuitButton");
			quitButton.Pressed += _on_quit_button_pressed;
		}

		public void _on_play_button_pressed()
		{
			if (isSceneChanging)
			{
				return;
			}

			GD.Print("Play Pressed");

			isSceneChanging = true;

            var tree = GetTree();
            if (tree != null)
            {
                tree.ChangeSceneToFile(_level1ScenePath);
                GD.Print("Play scene loaded");
            }
            else
            {
                GD.Print("Error: Tree is null");
				isSceneChanging = false;
            }
		}
		public void _on_settings_button_pressed()
		{
			settingsWindow.Popup();
		}

		public void _on_quit_button_pressed()
		{
			GD.Print("Quit Pressed");

			var tree = GetTree();
            if (tree != null)
            {
                tree.Quit();
            }
            else
            {
                GD.Print("Error: Tree is null");
            }
		}
	}
}

