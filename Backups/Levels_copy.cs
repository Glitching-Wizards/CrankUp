/*using Godot;
using System;

namespace CrankUp
{
	public partial class Levels : Node
	{
	[Export] private string StartLevel1 = "res://Menus/LevelStart/Scenes/StartLevel1.tscn";
	[Export] private string StartLevel2 = "res://Menus/LevelStart/Scenes/StartLevel2.tscn";
	[Export] private string StartLevel3 = "res://Menus/LevelStart/Scenes/StartLevel3.tscn";
	[Export] private string StartLevel4 = "res://Menus/LevelStart/Scenes/StartLevel4.tscn";
	[Export] private string StartLevel5 = "res://Menus/LevelStart/Scenes/StartLevel5.tscn";
	private int selectedLevel = 0;

	private Window StartWindow;
	private bool isSceneChanging = false;

			// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			TextureButton BackButton = GetNode<TextureButton>("Buttons/BackButton");
			BackButton.Pressed += BackButtonPressed;

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
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		public void BackButtonPressed()
		{
			GD.Print("Back Pressed");
			GetTree().ChangeSceneToFile("res://Menus/MainMenu/Scenes/MainMenu.tscn");
		}


		/// <summary>
		/// Katsotaan mikä leveli valitaan ja tuodaan esiin sen levelin pop up ikkuna.
		/// </summary>
		/// <param name="level">Valittu taso.</param>
		public void LevelButtonPressed(int level)
		{
			selectedLevel = level;

			string scenePath = string.Empty;
			Popup popup = null;

			switch(level)
			{
				case 1:
					StartLevel1.Popup();
					break;
				case 2:
					StartLevel2.Popup();
					break;
				case 3:
					StartLevel3.Popup();
					break;
				case 4:
					StartLevel4.Popup();
					break;
				case 5:
					StartLevel5.Popup();
					break;
				default:
					GD.Print("Error: No such level " + level);
					break;
			}
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
				2 => "res://Game/Level2/Scenes/StartLevel2.tscn",
				3 => "res://Game/Level3/Scenes/StartLevel3.tscn",
				4 => "res://Game/Level4/Scenes/StartLevel4.tscn",
				5 => "res://Game/Level5/Scenes/StartLevel5.tscn",
				_ => string.Empty
			};
		}
	}
}
*/
