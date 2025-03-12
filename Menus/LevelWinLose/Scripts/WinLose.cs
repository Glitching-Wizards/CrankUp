using Godot;
using System;

namespace CrankUp
{
    public partial class WinLose : Window
    {
        public override void _Ready()
        {
            TextureButton RetryButton = GetNode<TextureButton>("RetryButton");
            RetryButton.Pressed += RetryButtonPressed;

            TextureButton MenuButton = GetNode<TextureButton>("MenuButton");
            MenuButton.Pressed += MenuButtonPressed;

            //TextureButton NextButton = GetNode<TextureButton>("NextButton");
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

        public void MenuButtonPressed()
        {
            GD.Print("Menu Pressed");
            GetTree().ChangeSceneToFile("res://Menus/Levels/Scenes/Levels.tscn");
        }

        //public void NextButtonPressed()
        //{
        //    GD.Print("Next Pressed");
        // return level switch
        //{
        //    1 => "res://Menus/LevelStart/Scenes/StartLevel2.tscn",
        //   2 => "res://Menus/LevelStart/Scenes/StartLevel3.tscn",
        //   3 => "res://Menus/LevelStart/Scenes/StartLevel4.tscn",
        //   4 => "res://Menus/LevelStart/Scenes/StartLevel5.tscn",
        //   5 => "res://Menus/Levels/Scenes/Levels.tscn",
        //   _ => string.Empty
        //};
    }
}