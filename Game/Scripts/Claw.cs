using Godot;
using System;

public partial class Claw : Node2D
{
	public override void _Ready()
	{
		GlobalPosition = new Vector2(0, -360);
	}

	public override void _Process(double delta)
	{
	}
}
