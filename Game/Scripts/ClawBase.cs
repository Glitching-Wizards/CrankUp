using Godot;
using System;
namespace CrankUp;

/// <summary>
/// Represents the horizontal-moving base of the claw mechanism.
/// Handles left/right movement and ensures the attached ClawHead follows its X position.
/// </summary>
public partial class ClawBase : CharacterBody2D
{
	/// <summary>
    /// Base movement speed multiplier, adjustable in the Godot editor.
    /// </summary>
	[Export] private float speed = 10f;

	/// <summary>
    /// Stores the current calculated movement speed.
    /// </summary>
	private float _currentSpeed = 0f;

	/// <summary>
    /// Reference to the attached ClawHead node, which follows this base's horizontal movement.
    /// </summary>
	ClawHead clawHead;

	/// <summary>
    /// Called when the node enters the scene tree. Initializes references.
    /// </summary>
	public override void _Ready()
	{
		clawHead = GetNode<ClawHead>("ClawHead");
	}

	/// <summary>
    /// Moves the claw base horizontally based on player input.
    /// Applies clamped velocity and restricts movement within a horizontal boundary.
    /// </summary>
    /// <param name="direction">The movement direction (Vector2.Left or Vector2.Right).</param>
    /// <param name="speedFactor">Speed multiplier based on input intensity.</param>
    /// <param name="delta">Time since last frame.</param>
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

		// Clamp movement to remain within allowed screen boundaries
		GlobalPosition = new Vector2(Mathf.Clamp(GlobalPosition.X, -430, 380), GlobalPosition.Y);
	}

	/// <summary>
    /// Called every frame. Updates the ClawHead’s X position to follow the ClawBase.
    /// </summary>
    /// <param name="delta">Time since the last frame.</param>
	public override void _Process(double delta)
	{
		if (clawHead != null)
		{
			clawHead.GlobalPosition = new Vector2(GlobalPosition.X, clawHead.GlobalPosition.Y);
		}
	}
}
