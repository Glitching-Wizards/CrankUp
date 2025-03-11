using Godot;
using System;

namespace CrankUp
{
	public partial class MainMenu : Control
	{
		[Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";
		private bool isSceneChanging = false;
		public override void _Ready()
		{
			Button playButton = GetNode<Button>("Buttons/PlayButton");
			playButton.Pressed += _on_play_button_pressed;

			Button settingsButton = GetNode<Button>("Buttons/SettingsButton");
			settingsButton.Pressed += _on_settings_button_pressed;

			Button creditButton = GetNode<Button>("Buttons/CreditsButton");
			creditButton.Pressed += _on_credit_button_pressed;
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
                tree.ChangeSceneToFile(_levelsScenePath);
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
			if (isSceneChanging)
            {
				return;
			}
			GD.Print("Settings Pressed");

			isSceneChanging = true;

			var tree = GetTree();
            if (tree != null)
            {
                tree.ChangeSceneToFile(_settingsScenePath);
                GD.Print("Settings scene loaded");
            }
            else
            {
                GD.Print("Error: Tree is null");
				isSceneChanging = false;
            }
		}

		public void _on_credit_button_pressed()
		{
		}
	}
}

