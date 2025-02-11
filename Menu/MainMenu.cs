using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TextureButton playButton = GetNode<TextureButton>("Buttons/PlayButton");
		playButton.Pressed += _on_play_button_pressed;

		TextureButton optionsButton = GetNode<TextureButton>("Buttons/OptionsButton");
		optionsButton.Pressed += _on_options_button_pressed;

		TextureButton quitButton = GetNode<TextureButton>("Buttons/QuitButton");
		quitButton.Pressed += _on_quit_button_pressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_play_button_pressed()
	{
		GD.Print("Play Pressed");
		GetTree().ChangeSceneToFile("res://Game/game_start.tscn");
		GD.Print("Play scene loaded");
	}
	public void _on_options_button_pressed()
	{
		GD.Print("Options Pressed");
		GetTree().ChangeSceneToFile("res://Option/options.tscn");
		GD.Print("Option scene loaded");
	}

	public void _on_quit_button_pressed()
	{
		GD.Print("Quit Pressed");
		GetTree().Quit();
	}
}
