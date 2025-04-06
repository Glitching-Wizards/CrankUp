using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CrankUp
{

public partial class GameStart : Node2D
{
    public override void _Ready()
        {
            StartGame();
        }

        private async void StartGame()
        {
            GD.Print("Waiting for 3 seconds...");
            await ToSignal(GetTree().CreateTimer(3.0f), "timeout");

            GD.Print("Time is up! Changing scene...");
            GetTree().ChangeSceneToFile("res://Menus/MainMenu/Scenes/MainMenu.tscn");
        }
}
}
