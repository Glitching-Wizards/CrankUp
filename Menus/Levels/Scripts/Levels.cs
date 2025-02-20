using Godot;
using System;

namespace CrankUp
{
    public partial class Levels : Node
    {
        private int level = 1; //default level

		    // Called when the node enters the scene tree for the first time.
		    public override void _Ready()
        {
            TextureButton back_button = GetNode<TextureButton>("Buttons/BackButton");
            back_button.Pressed += _on_back_button_pressed;

            TextureButton level_button = GetNode<TextureButton>("Buttons/Level1");
            level_button.Pressed += _on_level_button_pressed;
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

        public void _on_level_button_pressed()
        {
            string scenePath = GetLevelScenePath(level);

            if (!string.IsNullOrEmpty(scenePath))
            {
                GetTree().ChangeSceneToFile(scenePath);
            }
            else
            {
                GD.PrintErr("Invalid level selected: " + level);
            }
        }

        private string GetLevelScenePath(int level)
        {
            return level switch
            {
                1 => "res://Game/Level1/Scenes/Level1.tscn",
                2 => "res://Game/Level2/Scenes/Level2.tscn",
                3 => "res://Game/Level3/Scenes/Level3.tscn",
                4 => "res://Game/Level4/Scenes/Level4.tscn",
                5 => "res://Game/Level5/Scenes/Level5.tscn",
                _ => string.Empty
            };
        }
    }
}

