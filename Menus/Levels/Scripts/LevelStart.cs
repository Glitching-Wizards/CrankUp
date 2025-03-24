using Godot;
using System;

namespace CrankUp
{
    public partial class LevelStart : Window
    {
		    // Called when the node enters the scene tree for the first time.
		public override void _Ready()
        {
            TextureButton PlayButton = GetNode<TextureButton>("PlayButton");
            PlayButton.Pressed += PlayButtonPressed;

            TextureButton MenuButton = GetNode<TextureButton>("MenuButton");
			MenuButton.Pressed += () => MenuButtonPressed();
        }


        public void MenuButtonPressed()
        {
            this.Hide();
        }

        public void PlayButtonPressed()
        {
            GD.Print("Play Pressed");

            CallDeferred(nameof(ChangeScene));
		}

		private void ChangeScene()
		{
			GetTree().ChangeSceneToFile("res://Game/Level1/Scenes/Level1.tscn");
		}
    }
}

