using Godot;
using System;
namespace CrankUp;
public partial class ClawBase : CharacterBody2D
{
	[Export] private float speed = 10f;
	private float _currentSpeed = 0f;
	ClawHead clawHead;
	public override void _Ready()
	{
		clawHead = GetNode<ClawHead>("ClawHead");
	}

	public void Move(Vector2 direction, float speedFactor, double delta)
	{
		if (direction != Vector2.Zero)
		{
			Velocity = direction * (speedFactor * speed) * (speedFactor / 50.0f);
		}
		else
		{
			Velocity = Vector2.Zero;
		}

		MoveAndSlide();

		GlobalPosition = new Vector2(Mathf.Clamp(GlobalPosition.X, -430, 380), GlobalPosition.Y); // Restrict X movement
	}

	public override void _Process(double delta)
	{
		if (clawHead != null)
		{
			clawHead.GlobalPosition = new Vector2(GlobalPosition.X, clawHead.GlobalPosition.Y);
		}
	}
}
