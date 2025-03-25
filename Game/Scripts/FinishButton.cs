using Godot;
using System;

namespace CrankUp;
public partial class FinishButton : Button
{
	public override void _Ready()
	{
		Pressed += () => {
		var victoryScreen = GetNode<Window>("/root/Level1/WinLose/Win");
		if (victoryScreen != null) victoryScreen.Popup();
		};
	}
}
