using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button playButton = GetNode<Button>("PlayButton");
		playButton.Pressed += _on_play_button_pressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_play_button_pressed()
	{
		GD.Print("Play Pressed");
		GetTree().ChangeSceneToFile("res://Game/game_start.tscn");
	}
}
