using Godot;
using System;
using System.Collections.Generic;

namespace CrankUp;
public partial class ClawHead : CharacterBody2D
{
	[Export] private float speed = 10f;
	private PinJoint2D joint;
	private Block grabbedBlock;
	private Area2D grabArea;
	private List<Block> nearbyBlocks = new List<Block>();
	private float _currentSpeed = 0f;
	private int currentGrabPointIndex = 0;  // Index to track the current grab point
	// List to hold the Marker2D grab points on the block
    private List<Marker2D> grabMarkers = new List<Marker2D>();

	public override void _Ready()
	{
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

		// Attach block to the position of the nearest Marker2D using a PinJoint2D
		joint = new PinJoint2D()
		{
			Name = "PinJoint2D",
			NodeA = GetPath(),
			NodeB = grabbedBlock.GetPath(),
			Position = grabMarker.GlobalPosition - GlobalPosition
		};

		// Add the joint as a child to the claw to keep it in the correct hierarchy
		AddChild(joint);
	}

	private Marker2D FindGrabMarkers(Block block)
	{
		// Get all Marker2D children of the block
        grabMarkers.Clear();
		Marker2D closestMarker = null;
		float closestDistance = float.MaxValue;

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

		if (grabMarkers.Count > 0)
		{
			return grabMarkers[currentGrabPointIndex];
		}
		
		return closestMarker;
	}

    public void DropBlock()
	{
		if (joint != null)
		{
			joint.QueueFree();
			joint = null;
		}
		grabbedBlock = null;
		currentGrabPointIndex = 0; // Reset grab point index on drop
	}

	public void RotateBlock()
    {
        if (grabbedBlock != null)
        {
           // Store the joint's global position before switching markers
			Vector2 oldJointPosition = joint.GlobalPosition;

			// Move to the next grab marker
			currentGrabPointIndex = (currentGrabPointIndex + 1) % grabMarkers.Count;
			GD.Print("Rotating to grab marker index: " + currentGrabPointIndex);

			// Update the joint position directly instead of regrabbing
			Marker2D newMarker = grabMarkers[currentGrabPointIndex];
			joint.GlobalPosition = oldJointPosition;
			joint.Position = newMarker.GlobalPosition - GlobalPosition;
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
		if (Input.IsActionJustPressed("Grab") && grabbedBlock == null)
			GrabBlock();

		if (Input.IsActionJustPressed("Drop") && grabbedBlock != null)
			DropBlock();
		
		if (Input.IsActionJustPressed("Rotate") && grabbedBlock != null)
			RotateBlock();
	}
}
