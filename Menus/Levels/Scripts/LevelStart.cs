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
            MenuButton.Pressed += MenuButtonPressed;
        }


        public void MenuButtonPressed()
        {
            GD.Print("Menu Pressed");
            GetTree().ChangeSceneToFile("res://Menus/MainMenu/Scenes/MainMenu.tscn");
        }

        public void PlayButtonPressed()
        {
            GD.Print("Play Pressed");
            GetTree().ChangeSceneToFile("res://Game/Level1/Scenes/Level1.tscn");

        }
    }
}

