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
	GD.Print("Starting _Ready...");

	// Debug: Print Root Children
	GD.Print("Children of root:");
	foreach (Node child in GetTree().Root.GetChildren()) {
		GD.Print("- " + child.Name);
	}

	// Debug: Print Level1 Children
	Node level1 = GetTree().Root.GetNodeOrNull("Level1");
	if (level1 != null)
	{
		GD.Print("Children of Level1:");
		foreach (Node child in level1.GetChildren()) {
			GD.Print("- " + child.Name);
		}
	}

	// Call deferred setup for PlacementArea & FinishButton
	CallDeferred(nameof(FindFinishButton));
	CallDeferred(nameof(FindPlacementArea));
}

private void FindFinishButton()
{
	endButton = GetNodeOrNull<Button>("/root/Level1/ConveyorBelt/ConveyorBelt3/FinishButton");
	if (endButton != null) 
		endButton.Pressed += OnButtonPressed;
	else
		GD.PrintErr("FinishButton still not found after defer!");
}

private void FindPlacementArea()
{
	placementArea = GetNodeOrNull<PlacementArea>("/root/Level1/PlacementArea");
	if (placementArea == null)
		GD.PrintErr("PlacementArea still not found after defer!");
}


		private void OnButtonPressed()
		{
			GD.Print("Finish Button Pressed! Checking score...");

			score = placementArea.GetScore();

			if (score >= 70 && score < 80)
			{
				GD.Print("Displaying Victory Screen 1 (70-80%)");
				if (victoryScreen1 != null) victoryScreen1.Visible = true;
			}
			else if (score >= 80 && score < 90)
			{
				GD.Print("Displaying Victory Screen 2 (80-90%)");
				if (victoryScreen2 != null) victoryScreen2.Visible = true;
			}
			else if (score >= 90)
			{
				GD.Print("Displaying Victory Screen 3 (90-100%)");
				if (victoryScreen3 != null) victoryScreen3.Visible = true;
			}
			else
			{
				GD.Print("No victory screen shown - score too low.");
			}
		}

		public void RetryButtonPressed() {
			GD.Print("Retry Pressed");
		}

		public void MenuButtonPressed() {
			GD.Print("Menu Pressed");
			GetTree().ChangeSceneToFile("res://Menus/Levels/Scenes/Levels.tscn");
		}
			private void Setup() {
			GD.Print("Running Setup...");
			placementArea = GetTree().Root.GetNodeOrNull<PlacementArea>("PlacementArea");
			if (placementArea == null)
				GD.PrintErr("PlacementArea still not found!");
		}
	}
}
