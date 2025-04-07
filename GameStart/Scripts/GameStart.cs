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

        // waits 3 seconds for the game start animation and then changes the scene to the main menu
        private async void StartGame()
        {
            await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
            GetTree().ChangeSceneToFile("res://Menus/MainMenu/Scenes/MainMenu.tscn");
        }
}
}
