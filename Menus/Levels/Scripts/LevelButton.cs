using Godot;
using System;

namespace CrankUp
{
	public partial class LevelButton : TextureButton
	{
		[Export] private int levelNumber;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			/*if (SaveSystem.GetGameData().LevelStars.ContainsKey(levelNumber))
			{
				TextureRect flagIcon = levelButton.GetNode<TextureRect>("Flag");
				if (flagIcon != null)
				{
					flagIcon.Visible = true;
				}
			}*/
	}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
