using Godot;
using System;

namespace CrankUp;
public partial class ClawHead : Sprite2D
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
	}

	private Vector2 ReadInput()
	{
		Vector2 moveDirection = Vector2.Zero;

		if (Input.IsActionPressed(Config.MoveUpAction)) moveDirection += Vector2.Up;
		if (Input.IsActionPressed(Config.MoveDownAction)) moveDirection += Vector2.Down;

		return moveDirection.Normalized(); // Normalize to prevent faster diagonal movement
	}

	private void Move(Vector2 direction, double delta)
	{
		Position += direction * speed * (float)delta; // Apply movement with delta
		GlobalPosition = new Vector2(GlobalPosition.X, Mathf.Clamp(GlobalPosition.Y, -291, 200)); // Restrict Y movement
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
