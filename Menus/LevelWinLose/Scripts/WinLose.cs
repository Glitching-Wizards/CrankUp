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
			endButton = GetNodeOrNull<Button>("/root/Level1/Background/ConveyerBelt/ConveyerBelt2/FinishButton");

			victoryScreen1 = GetNodeOrNull<Window>("Win");
			victoryScreen2 = GetNodeOrNull<Window>("Win2");
			victoryScreen3 = GetNodeOrNull<Window>("Win3");

			if (victoryScreen1 != null) victoryScreen1.Visible = false;
			if (victoryScreen2 != null) victoryScreen2.Visible = false;
			if (victoryScreen3 != null) victoryScreen3.Visible = false;

			CallDeferred(nameof(Setup));

			placementArea = GetNodeOrNull<PlacementArea>("/root/Level1/PlacementArea"); 
			if (placementArea == null)
				GD.PrintErr("PlacementArea not found!");

			if (endButton != null) 
				endButton.Pressed += OnButtonPressed;
			else
				GD.PrintErr("FinishButton not found!");

			TextureButton RetryButton = GetNodeOrNull<TextureButton>("RetryButton");
			if (RetryButton != null) RetryButton.Pressed += RetryButtonPressed;

			TextureButton MenuButton = GetNodeOrNull<TextureButton>("MenuButton");
			if (MenuButton != null) MenuButton.Pressed += MenuButtonPressed;

			if (placementArea == null)
			GD.PrintErr("PlacementArea not found in the scene!");

			if (endButton == null)
			GD.PrintErr("FinishButton not found in the scene!");

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
   		 placementArea = GetTree().Root.GetNodeOrNull<PlacementArea>("Level1/PlacementArea");
   		 if (placementArea == null)
		GD.PrintErr("PlacementArea not found even after defer!");
		}
	}
}
