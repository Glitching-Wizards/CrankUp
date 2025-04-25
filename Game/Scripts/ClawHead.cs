using Godot;
using System;
using System.Collections.Generic;

namespace CrankUp;

/// <summary>
/// Controls the vertical movement of the claw head and its ability to grab, hold, rotate, and drop blocks.
/// </summary>
public partial class ClawHead : CharacterBody2D
{
	/// <summary>
    /// Movement speed factor for vertical movement.
    /// </summary>
	[Export] private float speed = 10f;

	private Sprite2D clawSprite;

	/// <summary>
    /// Joint used to attach the grabbed block to the claw.
    /// </summary>
	private PinJoint2D joint;

	/// <summary>
    /// Collision shape of the claw head, used for enabling/disabling during grabs.
    /// </summary>
	public CollisionShape2D collisionShape;

	/// <summary>
    /// The currently grabbed block, if any.
    /// </summary>
	public Block grabbedBlock;

	/// <summary>
    /// Area used to detect nearby blocks for grabbing.
    /// </summary>
	private Area2D grabArea;

	/// <summary>
    /// List of all blocks currently within the grab area.
    /// </summary>
	private List<Block> nearbyBlocks = new List<Block>();

	/// <summary>
    /// Stores the current calculated movement speed.
    /// </summary>
	private float _currentSpeed = 0f;

	/// <summary>
    /// Cache of Marker2D grab points on a block.
    /// </summary>
	private List<Marker2D> grabMarkers = new List<Marker2D>();

	/// <summary>
    /// Called when the node enters the scene tree.
    /// Sets up references and connects area signals.
    /// </summary>
	public override void _Ready()
	{
		clawSprite = GetNodeOrNull<Sprite2D>("Sprite2D");
		collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");

		grabArea = GetNode<Area2D>("GrabArea");

		grabArea.BodyEntered += OnBodyEntered;
		grabArea.BodyExited += OnBodyExited;
	}

	/// <summary>
    /// Called when a body enters the grab area.
    /// Adds it to the list of nearby blocks if it is a valid block.
    /// </summary>
	private async void OnBodyEntered(Node2D body)
	{
		if (body is Block block && !nearbyBlocks.Contains(block))
 		{
 			nearbyBlocks.Add(block);
 			clawSprite.Frame = 1;
 			await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
 			clawSprite.Frame = 2;
 		}
	}

	/// <summary>
    /// Called when a body exits the grab area.
    /// Removes it from the list of nearby blocks.
    /// </summary>
	private async void OnBodyExited(Node2D body)
	{
		if (body is Block block)
		{
 			nearbyBlocks.Remove(block);
 			clawSprite.Frame =1;
 			await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
 			clawSprite.Frame = 0;
 		}
	}

	/// <summary>
    /// Attempts to grab a block in the nearby area, or drops the current block if one is held.
    /// </summary>
	public void GrabBlock()
	{
		if (grabbedBlock != null)
		{
			DropBlock();
			return;
		}

		if (nearbyBlocks.Count == 0)
		{
			GD.Print("No blocks nearby!");
			return;
		}

		grabbedBlock = nearbyBlocks[0];

		// Find the nearest Marker2D on the block
		Marker2D grabMarker = FindGrabMarkers(grabbedBlock);

		// Align block with claw at the marker position
		grabbedBlock.GlobalPosition += GlobalPosition - grabMarker.GlobalPosition;

		// Attach block to the position of the nearest Marker2D using a PinJoint2D
		joint = new PinJoint2D()
		{
			Name = "PinJoint2D",
			NodeA = GetPath(),
			NodeB = grabbedBlock.GetPath(),
			Position = grabMarker.GlobalPosition - GlobalPosition,
		};

		// Disable collision while grabbing
		collisionShape.SetDeferred("disabled", true);

		// Add the joint as a child to the claw
		AddChild(joint);
	}

	/// <summary>
    /// Finds the closest Marker2D on a block to use as the grab point.
    /// </summary>
    /// <param name="block">The block to search for markers.</param>
    /// <returns>The closest Marker2D to the claw's position.</returns>
	private Marker2D FindGrabMarkers(Block block)
	{
		// Get all Marker2D children of the block
		grabMarkers.Clear();
		Marker2D closestMarker = null;
		float closestDistance = float.MaxValue;

		// Find the closest marker
		foreach (Node child in block.GetChildren())
		{
			if (child is Marker2D marker)
			{
				grabMarkers.Add(marker);
				float distance = GlobalPosition.DistanceTo(marker.GlobalPosition);

				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestMarker = marker;
				}
			}
		}
		return closestMarker;
	}

	/// <summary>
    /// Drops the currently grabbed block and re-enables collisions after a short delay.
    /// </summary>
    public async void DropBlock()
	{
		if (joint != null)
		{
			joint.QueueFree();
			joint = null;
		}
		grabbedBlock = null;

		await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
        collisionShape.SetDeferred("disabled", false);

	}

	/// <summary>
    /// Rotates the grabbed block counterclockwise by 90 degrees and re-grabs it.
    /// </summary>
	public void RotateBlock()
	{
		if (grabbedBlock != null)
		{
		   // Rotate counterclockwise
			grabbedBlock.RotationDegrees -= 90;

			// Re-grab the block
			if (joint != null)
			{
				joint.QueueFree();
				joint = null;
			}

			grabbedBlock = null;
			GrabBlock();
		}
	}

	/// <summary>
    /// Moves the claw head vertically within a restricted range.
    /// </summary>
    /// <param name="direction">The movement direction (Vector2.Up or Vector2.Down).</param>
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
		GlobalPosition = new Vector2(GlobalPosition.X, Mathf.Clamp(GlobalPosition.Y, -291, 200)); // Restrict Y movement
	}

	/// <summary>
    /// Frame update (currently unused).
    /// </summary>
	public override void _Process(double delta)
	{
	}
}
