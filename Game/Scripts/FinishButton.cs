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

			ControlsLeftUi leftUi = currentLevel.FindChild("ControlsLeftUi", true, false) as ControlsLeftUi;
			if (leftUi != null)
			{
				leftUi.TimeRanOut += OnTimeRanOut;
			}

			FindPlacementArea();

			Pressed += OnButtonPressed;
			CallDeferred(nameof(ConnectTimerSignal));
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

			if (score >= 50 && score < 80 && victoryScreen1 != null)
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

			GD.PrintErr($"Could not parse level number from level name: {levelName}");
			return 0;
		}

		private void ConnectTimerSignal()
		{
			ControlsLeftUi leftUi = currentLevel.FindChild("ControlsLeftUi", true, false) as ControlsLeftUi;
			if (leftUi != null)
			{
				leftUi.TimeRanOut += OnTimeRanOut;
				GD.Print("Connected to TimeRanOut");
			}
			else
			{
				GD.PrintErr("Could not find ControlsLeftUi to connect signal.");
			}
		}

		public void ScoreAtTimeout()
		{
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
		}
	}
}
