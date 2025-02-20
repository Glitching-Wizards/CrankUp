using Godot;
using System;

namespace CrankUp
{
	public partial class MainMenu : Control
	{
		[Export] private string _level1ScenePath = "res://Game/Level1/Scenes/Level1.tscn";
		[Export] private string _optionsScenePath = "res://Menus/Options/Scenes/Options.tscn";
		private bool isSceneChanging = false;
		public override void _Ready()
		{
			TextureButton playButton = GetNode<TextureButton>("Buttons/PlayButton");
			playButton.Pressed += _on_play_button_pressed;

			TextureButton optionsButton = GetNode<TextureButton>("Buttons/OptionsButton");
			optionsButton.Pressed += _on_options_button_pressed;

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

