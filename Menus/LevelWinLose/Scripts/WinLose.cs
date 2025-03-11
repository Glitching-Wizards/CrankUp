using Godot;
using System;

namespace CrankUp
{
    public partial class WinLose : Window
    {
        public override void _Ready()
        {
            TextureButton RetryButton = GetNode<TextureButton>("RetryButton");
            RetryButton.Pressed += _on_retry_button_pressed;

            TextureButton MenuButton = GetNode<TextureButton>("MenuButton");
            MenuButton.Pressed += _on_menu_button_pressed;

            //TextureButton NextButton = GetNode<TextureButton>("NextButton");
            //NextButton.Pressed += _on_next_button_pressed;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }

        public void _on_retry_button_pressed()
        {
            GD.Print("Retry Pressed");
            GetTree().ChangeSceneToFile("res://Game/Level1/Scenes/Level1.tscn");
        }

        public void _on_menu_button_pressed()
        {
            GD.Print("Menu Pressed");
            GetTree().ChangeSceneToFile("res://Menus/Levels/Scenes/Levels.tscn");
        }

        //public void _on_next_button_pressed()
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