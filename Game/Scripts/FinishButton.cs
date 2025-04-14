using Godot;
using System;
using System.Linq;

namespace CrankUp
{
	public partial class FinishButton : Button
	{
		private Window victoryScreen1, victoryScreen2, victoryScreen3, loseScreen;
		private PlacementArea placementArea;
		private float score;
		private Node currentLevel;
		[Export] private AudioStream loseSound = GD.Load<AudioStream>("res://Audio/Lose.mp3");
		[Export] private AudioStream winSound = GD.Load<AudioStream>("res://Audio/Win.mp3");

		public override void _Ready() {
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

            Ui ui = currentLevel.GetNodeOrNull<Ui>("Ui");
            if (ui != null)
            {
                ui.TimeRanOut += OnTimeRanOut;
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
			if (placementArea == null) return;

			score = placementArea.GetScore();
			int stars = 0;

			if (score >= 70 && score < 80 && victoryScreen1 != null)
			{
				stars = 1;
				victoryScreen1.Visible = true;
<<<<<<< HEAD
=======
				AudioManager.PlaySound(winSound);
>>>>>>> 407834f9439e6171bd94341ac98b85bd1cbe1b64
			}
			else if (score >= 80 && score < 90 && victoryScreen2 != null)
			{
				stars = 2;
				victoryScreen2.Visible = true;
<<<<<<< HEAD
=======
				AudioManager.PlaySound(winSound);
>>>>>>> 407834f9439e6171bd94341ac98b85bd1cbe1b64
			}
			else if (score >= 90 && victoryScreen3 != null)
			{
				stars = 3;
				victoryScreen3.Visible = true;
<<<<<<< HEAD
=======
				AudioManager.PlaySound(winSound);
>>>>>>> 407834f9439e6171bd94341ac98b85bd1cbe1b64
			}
			else if (score < 70 && loseScreen != null)
			{
				loseScreen.Visible = true;
<<<<<<< HEAD
=======
				AudioManager.PlaySound(loseSound);
>>>>>>> 407834f9439e6171bd94341ac98b85bd1cbe1b64
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
			// does it get all the buttons
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

		private void OnTimeRanOut() {
            if (loseScreen != null)
            {
                loseScreen.Visible = true;
            }
        }

		private int GetLevelNumberFromName(string levelName)
		{
			var digits = new string(levelName.Where(char.IsDigit).ToArray());
			if (int.TryParse(digits, out int number))
				return number;
<<<<<<< HEAD
			
=======

>>>>>>> 407834f9439e6171bd94341ac98b85bd1cbe1b64
			GD.PrintErr($"Could not parse level number from level name: {levelName}");
			return 0;
		}
	}
}