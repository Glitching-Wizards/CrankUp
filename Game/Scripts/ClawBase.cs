using Godot;
using System;
namespace CrankUp;
public partial class ClawBase : Sprite2D
{
	[Export] private float speed = 100;
	public override void _Ready()
	{
	}

	private Vector2 ReadInput()
	{
		Vector2 moveDirection = Vector2.Zero;

		if (Input.IsActionPressed(Config.MoveRightAction)) moveDirection += Vector2.Right;
		if (Input.IsActionPressed(Config.MoveLeftAction)) moveDirection += Vector2.Left;

		return moveDirection.Normalized();
	}

	private void Move(Vector2 direction, double delta)
	{
		Position += direction * speed * (float)delta; // Apply movement with delta
		GlobalPosition = new Vector2(Mathf.Clamp(GlobalPosition.X, -480, 200), GlobalPosition.Y); // Restrict X movement
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
