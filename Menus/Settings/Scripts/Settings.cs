using Godot;
using System;

namespace CrankUp
{
<<<<<<< HEAD:Menus/Settings/Scripts/Settings.cs
	public partial class Settings : Window
=======
	public partial class Options : Window
>>>>>>> origin/Jasmin:Menus/Options/Scripts/Options.cs
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
			this.Hide();
		}
	}
}
