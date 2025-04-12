using Godot;
using System;
using System.Linq;

namespace CrankUp
{
	public partial class FinishButton : Button
	{
		[Export] private string _levelsScenePath = "res://Menus/Levels/Scenes/Levels.tscn";
		private Node _levelsInstance;
		private Window victoryScreen1, victoryScreen2, victoryScreen3, loseScreen;
		private PlacementArea placementArea;
		private float score;
		private Node currentLevel;
		[Export] private AudioStream clickSound;

		public override void _Ready()
		{
			currentLevel = GetTree().CurrentScene;
			if (currentLevel == null) return;

			Node winLose = currentLevel.GetNodeOrNull("WinLose");
			if (winLose != null)
			{
				victoryScreen1 = winLose.GetNodeOrNull<Window>("Win");
				victoryScreen2 = winLose.GetNodeOrNull<Window>("Win2");
				victoryScreen3 = winLose.GetNodeOrNull<Window>("Win3");
				loseScreen = winLose.GetNodeOrNull<Window>("Lose");

				if (victoryScreen1 != null) victoryScreen1.Visible = false;
				if (victoryScreen2 != null) victoryScreen2.Visible = false;
				if (victoryScreen3 != null) victoryScreen3.Visible = false;
				if (loseScreen != null) loseScreen.Visible = false;
			}

			Pressed += OnButtonPressed;
			CallDeferred(nameof(FindPlacementArea));
		}

		private void FindPlacementArea()
		{
			placementArea = currentLevel.GetNodeOrNull<PlacementArea>("PlacementArea");
		}

		private void OnButtonPressed()
		{
			AudioManager.PlaySound(clickSound);
			if (placementArea == null) return;

			score = placementArea.GetScore();
			int stars = 0;

			if (score >= 70 && score < 80 && victoryScreen1 != null)
			{
				stars = 1;
				victoryScreen1.Visible = true;
			}
			else if (score >= 80 && score < 90 && victoryScreen2 != null)
			{
				stars = 2;
				victoryScreen2.Visible = true;
			}
			else if (score >= 90 && victoryScreen3 != null)
			{
				stars = 3;
				victoryScreen3.Visible = true;
			}
			else if (score < 70 && loseScreen != null)
			{
				loseScreen.Visible = true;
				return;
			}

			if (stars > 0)
			{
				int levelNumber = GetLevelNumberFromName(currentLevel.Name);
				SaveSystem.OnLevelCompleted(levelNumber, stars);
			}

			LevelDone(currentLevel.Name);
		}

		private void LevelDone(string levelName)
		{
			Node levelButtonPath = GetTree().Root.GetNode<Node>("/root/Menus/Levels/Scenes/Levels.tscn/Levels/Buttons");

			if (levelButtonPath == null)
			{
				GD.PrintErr($"Virhe: Node 'Levels/Buttons' ei löydy.");
				return;
			}

			if (levelButtonPath != null && levelButtonPath.HasNode(levelName))
			{
				TextureButton levelButton = levelButtonPath.GetNode<TextureButton>(levelName);

				levelButton.Disabled = false;

				TextureRect flagIcon = levelButton.GetNode<TextureRect>("Flag");
				if (flagIcon != null)
				{
					flagIcon.Visible = true;
				}

				TextureRect number = levelButton.GetNode<TextureRect>("Number");
				if (number != null)
				{
					number.Visible = true;
				}
			}
		}

		private int GetLevelNumberFromName(string levelName)
		{
			var digits = new string(levelName.Where(char.IsDigit).ToArray());
			if (int.TryParse(digits, out int number))
				return number;
			
			GD.PrintErr($"Could not parse level number from level name: {levelName}");
			return 0;
		}
	}
}