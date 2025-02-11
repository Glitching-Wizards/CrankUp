using Godot;
using System;

namespace CraneGame;
public partial class Claw : Node2D
{
	[Export] public int speed = 100;

	public enum Direction
	{
		None = 0,
		Up,
		Down,
		Right,
		Left
	}

	public override void _Ready()
	{
		Position = new Vector2(-3, -269);
	}

	private Vector2 ReadInput()
	{
		Vector2 moveDirection = Vector2.Zero;

		if (Input.IsActionPressed(Config.MoveUpAction)) moveDirection += Vector2.Up;
		if (Input.IsActionPressed(Config.MoveDownAction)) moveDirection += Vector2.Down;
		if (Input.IsActionPressed(Config.MoveRightAction)) moveDirection += Vector2.Right;
		if (Input.IsActionPressed(Config.MoveLeftAction)) moveDirection += Vector2.Left;

		return moveDirection.Normalized(); // Normalize to prevent faster diagonal movement
	}

	private void Move(Vector2 direction, double delta)
	{
		Position += direction * speed * (float)delta; // Apply movement with delta
	}

	public override void _Process(double delta)
	{
		Vector2 direction = ReadInput();
		if (direction != Vector2.Zero)
		{
			Move(direction, delta);
		}
	}
}
