using Godot;
using System;

namespace CrankUp
{
    public partial class Levels : Node
    {
    [Export] private string _startLevelScenePath = "res://Menus/LevelStart/Scenes/StartLevel1.tscn";
    private Window startLevelWindow;

		    // Called when the node enters the scene tree for the first time.
		public override void _Ready()
        {
            PackedScene startScene = (PackedScene)GD.Load(_startLevelScenePath);
    		startLevelWindow = (Window)startScene.Instantiate();
    		AddChild(startLevelWindow);

    		startLevelWindow.Visible = false;

            TextureButton LevelButton1 = GetNode<TextureButton>("Buttons/Level1");
            LevelButton1.Pressed += () => LevelButtonPressed(1);

            TextureButton LevelButton2 = GetNode<TextureButton>("Buttons/Level2");
            LevelButton2.Pressed += () => LevelButtonPressed(2);

            TextureButton LevelButton3 = GetNode<TextureButton>("Buttons/Level3");
            LevelButton3.Pressed += () => LevelButtonPressed(3);

            TextureButton LevelButton4 = GetNode<TextureButton>("Buttons/Level4");
            LevelButton4.Pressed += () => LevelButtonPressed(4);

            TextureButton LevelButton5 = GetNode<TextureButton>("Buttons/Level5");
            LevelButton5.Pressed += () => LevelButtonPressed(5);

            TextureButton Back = GetNode<TextureButton>("Buttons/BackButton");
            Back.Pressed += () => BackButtonPressed();

            TextureButton Settings = GetNode<TextureButton>("Buttons/SettingsButton");
            Settings.Pressed += () => SettingsButtonPressed();
        }


        public void BackButtonPressed()
        {
            GD.Print("Back Pressed");
            GetTree().ChangeSceneToFile("res://Menus/MainMenu/Scenes/MainMenu.tscn");
        }

        public void SettingsButtonPressed()
        {
            GD.Print("Settings Pressed");
            GetTree().ChangeSceneToFile("res://Menus/Settings/Scenes/Settings.tscn");
        }



        public void LevelButtonPressed(int level)
        {
            GD.Print($"Level {level} button pressed");

            string scenePath = GetLevelScenePath(level);

            startLevelWindow.Popup();
        }

        /// <summary>
        /// Valitun tason perusteella avataan oikea taso scene, kun pelaajaa painaa play.
        /// </summary>
        /// <param name="level">Valittu taso.</param>
        /// <returns></returns>
        private string GetLevelScenePath(int level)
        {
            return level switch
            {
                1 => "res://Menus/LevelStart/Scenes/StartLevel1.tscn",
                2 => "res://Menus/LevelStart/Scenes/StartLevel2.tscn",
                3 => "res://Menus/LevelStart/Scenes/StartLevel3.tscn",
                4 => "res://Menus/LevelStart/Scenes/StartLevel4.tscn",
                5 => "res://Menus/LevelStart/Scenes/StartLevel5.tscn",
                _ => string.Empty
            };
        }
    }
}

