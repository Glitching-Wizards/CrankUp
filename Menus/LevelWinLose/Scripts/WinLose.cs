using Godot;
using System;

namespace CrankUp
{
	public partial class WinLose : Window
	{
		private Button endButton;
		private Window victoryScreen1, victoryScreen2, victoryScreen3;
		private PlacementArea placementArea;
		private float score;

		public override void _Ready()
		{
			Node level1 = GetTree().Root.GetNodeOrNull("Level1");
			if (level1 != null)
			{
				victoryScreen1 = level1.GetNodeOrNull<Window>("WinLose/Win");
				victoryScreen2 = level1.GetNodeOrNull<Window>("WinLose/Win2");
				victoryScreen3 = level1.GetNodeOrNull<Window>("WinLose/Win3");

				if (victoryScreen1 != null) victoryScreen1.Visible = false;
				if (victoryScreen2 != null) victoryScreen2.Visible = false;
				if (victoryScreen3 != null) victoryScreen3.Visible = false;
			}

			CallDeferred(nameof(FindFinishButton));
			CallDeferred(nameof(FindPlacementArea));
		}

		private void FindFinishButton()
		{
			endButton = GetNodeOrNull<Button>("/root/Level1/ConveyorBelt/ConveyorBelt3/FinishButton");

			if (endButton != null)
				endButton.Pressed += OnButtonPressed;
		}

		private void FindPlacementArea()
		{
			placementArea = GetNodeOrNull<PlacementArea>("/root/Level1/PlacementArea");
		}

		private void OnButtonPressed()
		{
			score = placementArea.GetScore();

			if (score >= 70 && score < 80 && victoryScreen1 != null)
				victoryScreen1.Visible = true;
			else if (score >= 80 && score < 90 && victoryScreen2 != null)
				victoryScreen2.Visible = true;
			else if (score >= 90 && victoryScreen3 != null)
				victoryScreen3.Visible = true;
		}

		public void RetryButtonPressed()
		{
			GD.Print("Retry Pressed");
			GetTree().ReloadCurrentScene();
		}

		public void MenuButtonPressed()
		{
			GetTree().ChangeSceneToFile("res://Menus/Levels/Scenes/Levels.tscn");
		}
	}
}
