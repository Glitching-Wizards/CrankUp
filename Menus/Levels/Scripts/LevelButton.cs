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
		}

		public void ShowFlagAndNumber()
		{
			if (SaveSystem.GetGameData().LevelStars.ContainsKey(levelNumber))
			{
				TextureRect flagIcon = GetNode<TextureRect>("Flag");
				if (flagIcon != null)
				{
					flagIcon.Visible = true;
				}

				TextureRect number = GetNode<TextureRect>("Number");
				if (number != null)
				{
					number.Visible = true;
				}
			}
		}
	}
}
