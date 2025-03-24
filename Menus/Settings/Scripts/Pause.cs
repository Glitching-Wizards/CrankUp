using Godot;
using System;

namespace CrankUp
{
    public partial class Pause : Node
    {
        private Button endButton;
        private Window victoryScreen;
        public override void _Ready()
        {
            TextureButton RetryButton = GetNode<TextureButton>("Buttons/RetryButton");
            RetryButton.Pressed += RetryButtonPressed;

            TextureButton MenuButton = GetNode<TextureButton>("Buttons/MenuButton");
            MenuButton.Pressed += MenuButtonPressed;

            //TextureButton NextButton = GetNode<TextureButton>("Buttons/OptionsButton");
            //NextButton.Pressed += NextButtonPressed;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        public void RetryButtonPressed()
        {
            GD.Print("Retry Pressed");

            // return level switch
            //{
            //    1 => "res://Game/Level1/Scenes/Level1.tscn",
            //   2 => "res://Game/Level1/Scenes/Level2.tscn",
            //   3 => "res://Game/Level1/Scenes/Level3.tscn",
            //   4 => "res://Game/Level1/Scenes/Level4.tscn",
            //   5 => "res://Game/Level1/Scenes/Level5.tscn",
            //   _ => string.Empty
        }

        private void OnButtonPressed()
        {
            GD.Print("TOIMII");
            victoryScreen.Visible = true;
        }

        public void MenuButtonPressed()
        {
            GD.Print("Menu Pressed");
            GetTree().ChangeSceneToFile("res://Menus/Levels/Scenes/Levels.tscn");
        }
    }
}

