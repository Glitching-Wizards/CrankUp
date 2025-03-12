using Godot;
using System;

namespace CrankUp
{
	public partial class Settings : Window
	{
		public override void _Ready()
		{
			TextureButton backButton = GetNode<TextureButton>("BackButton");
			backButton.Pressed += () => BackButtonPressed();

			Button quitButton = GetNode<Button>("QuitButton");
			if (quitButton == null)
			{
				GD.PrintErr("[ERROR] QuitButton not found in Settings.tscn");
			}
			else
			{
				quitButton.Pressed += QuitButtonPressed;
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public void BackButtonPressed()
		{
			this.Hide();
		}

		public void QuitButtonPressed()
		{
			GD.Print("Quit Pressed");

			if (!this.IsInsideTree())
			{
				GD.PrintErr("[ERROR] Settings scene is not inside tree.");
				return;
			}

			this.Visible = false;

			CallDeferred(nameof(ChangeScene));
		}

		private void ChangeScene()
		{
			GetTree().ChangeSceneToFile("res://Menus/Levels/Scenes/Levels.tscn");
		}
	}
}
