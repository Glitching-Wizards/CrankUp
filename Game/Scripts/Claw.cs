using Godot;
using System;

namespace CrankUp;

/// <summary>
/// The Claw class is a Node2D used by the claw root node in the game.
/// It represents the claw mechanism's main visual and positional component.
/// </summary>
public partial class Claw : Node2D
{
	/// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Initializes the claw's global position.
    /// </summary>
	public override void _Ready()
	{
		GlobalPosition = new Vector2(0, -360); // Sets the claws initial position
	}
}
