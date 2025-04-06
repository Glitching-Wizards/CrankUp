using Godot;
using System;

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

			Pressed += OnButtonPressed;
			CallDeferred(nameof(FindPlacementArea));
		}

		private void FindPlacementArea() {
			placementArea = currentLevel.GetNodeOrNull<PlacementArea>("PlacementArea");
		}

		private void OnButtonPressed() {
			if (placementArea == null) return;

			score = placementArea.GetScore();

			if (score >= 70 && score < 80 && victoryScreen1 != null)
				victoryScreen1.Visible = true;
			else if (score >= 80 && score < 90 && victoryScreen2 != null)
				victoryScreen2.Visible = true;
			else if (score >= 90 && victoryScreen3 != null)
				victoryScreen3.Visible = true;
			else if (score < 70 && loseScreen != null)
				loseScreen.Visible = true;
		}

		public void RetryButtonPressed() {
			GetTree().ReloadCurrentScene();
		}

		public void MenuButtonPressed() {
			GetTree().ChangeSceneToFile("res://Menus/Levels/Scenes/Levels.tscn");
		}
	}
}
