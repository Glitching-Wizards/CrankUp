using Godot;
using System;

namespace CrankUp
{
	public partial class game_start : Node2D
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			TextureButton backButton = GetNode<TextureButton>("back_button");
			backButton.Pressed += _on_back_button_pressed;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public void _on_back_button_pressed()
		{
			GD.Print("Back Pressed");
			GetTree().ChangeSceneToFile("res://menu/main_menu.tscn");
		}
	}
}
