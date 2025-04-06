using Godot;
using System;
using System.Collections.Generic;

namespace CrankUp;
public partial class ClawHead : CharacterBody2D
{
	[Export] private float speed = 10f;
	private PinJoint2D joint;
	public CollisionShape2D collisionShape;
	public Block grabbedBlock;
	private Area2D grabArea;
	private List<Block> nearbyBlocks = new List<Block>();
	private float _currentSpeed = 0f;

	private List<Marker2D> grabMarkers = new List<Marker2D>(); // List to hold the Marker2D grab points on the block

	public override void _Ready()
	{
		collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");

		grabArea = GetNode<Area2D>("GrabArea");

		grabArea.BodyEntered += OnBodyEntered;
		grabArea.BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Block block && !nearbyBlocks.Contains(block))
			nearbyBlocks.Add(block);
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is Block block)
			nearbyBlocks.Remove(block);
	}

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

		// Grabbed block is aligned with claw
		grabbedBlock.GlobalPosition += GlobalPosition - grabMarker.GlobalPosition;

		// Attach block to the position of the nearest Marker2D using a PinJoint2D
		joint = new PinJoint2D()
		{
			Name = "PinJoint2D",
			NodeA = GetPath(),
			NodeB = grabbedBlock.GetPath(),
			Position = grabMarker.GlobalPosition - GlobalPosition,
		};

		collisionShape.SetDeferred("disabled", true);  // Disable collision

		// Add the joint as a child to the claw
		AddChild(joint);
	}

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

		GlobalPosition = new Vector2(GlobalPosition.X, Mathf.Clamp(GlobalPosition.Y, -291, 200)); // Restrict Y movement
	}

	public override void _Process(double delta)
	{
	}
}
