using Godot;
using System;

namespace CrankUp
{
	public partial class Options : Control
	{
		public override void _Ready()
		{
			TextureButton backButton = GetNode<TextureButton>("BackButton");
			backButton.Pressed += _on_back_button_pressed;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public void _on_back_button_pressed()
		{
			GD.Print("Back Pressed");
			GetTree().ChangeSceneToFile("res://Menus/MainMenu/Scenes/MainMenu.tscn");
		}
	}
}
