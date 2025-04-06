using Godot;
using System;

namespace CrankUp;
public partial class Claw : Node2D
{
	public override void _Ready()
	{
		GlobalPosition = new Vector2(0, -360);
	}
}
