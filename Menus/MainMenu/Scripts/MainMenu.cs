using Godot;
using System;

namespace CrankUp
{
	public partial class MainMenu : Control
	{
		[Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		[Export] private string _optionsScenePath = "res://Menus/Options/Scenes/Options.tscn";
		private bool isSceneChanging = false;
		public override void _Ready()
		{
			Button playButton = GetNode<Button>("Buttons/PlayButton");
			playButton.Pressed += _on_play_button_pressed;

			Button optionsButton = GetNode<Button>("Buttons/OptionsButton");
			optionsButton.Pressed += _on_options_button_pressed;

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
		public void _on_options_button_pressed()
		{
			if (isSceneChanging)
            {
				return;
			}
			GD.Print("Options Pressed");

			isSceneChanging = true;

			var tree = GetTree();
            if (tree != null)
            {
                tree.ChangeSceneToFile(_optionsScenePath);
                GD.Print("Option scene loaded");
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

