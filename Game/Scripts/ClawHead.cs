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
		if (nearbyBlocks.Count == 0)
		{
			GD.Print("No blocks nearby!");
			return;
		}

		grabbedBlock = nearbyBlocks[0];
		GD.Print("Grabbed block: " + grabbedBlock.Name);

		joint = new PinJoint2D();
		joint.Name = "PinJoint2D";
		GlobalPosition = GlobalPosition;
		joint.NodeA = GetPath();
		joint.NodeB = grabbedBlock.GetPath();

		AddChild(joint);
	}

	public void DropBlock()
	{
		if (joint != null)
		{
			joint.QueueFree();
			joint = null;
		}
		grabbedBlock = null;
	}

	private Vector2 ReadInput()
	{
		Vector2 moveDirection = Vector2.Zero;

		if (Input.IsActionPressed(Config.MoveUpAction)) moveDirection += Vector2.Up;
		if (Input.IsActionPressed(Config.MoveDownAction)) moveDirection += Vector2.Down;

		return moveDirection.Normalized(); // Normalize to prevent faster diagonal movement
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
	}
}
