using Godot;

public partial class VictoryPopUp : Control
{
	private Button endButton;
	private TextureRect victoryScreen;

	public override void _Ready() {
		endButton = GetNode<Button>("/root/Level1/Ui/Button"); 
		victoryScreen = GetNode<TextureRect>("VictoryScreen");

		if (victoryScreen != null) 
			victoryScreen.Visible = false;
		
		if (endButton != null) 
			endButton.Pressed += OnButtonPressed;
	}

	private void OnButtonPressed() {
		victoryScreen.Visible = true;
	}
}
