using Godot;
using System.Collections.Generic;

public partial class Rope : CharacterBody2D
{
	[Export] private float _speed = 100f;
	private PinJoint2D joint;
	private Block grabbedBlock;
	private Area2D grabArea;
	private List<Block> nearbyBlocks = new List<Block>();

	public override void _Ready()
	{
		grabArea = GetNode<Area2D>("GrabArea");

		grabArea.BodyEntered += OnBodyEntered;
		grabArea.BodyExited += OnBodyExited;
	}

	public override void _Process(double delta)
	{
		Vector2 velocity = Vector2.Zero;

		if (Input.IsActionPressed("Left"))
			velocity.X -= (float)(_speed * delta);
		if (Input.IsActionPressed("Right"))
			velocity.X += (float)(_speed * delta);
		if (Input.IsActionPressed("Up"))
			velocity.Y -= (float)(_speed * delta);
		if (Input.IsActionPressed("Down"))
			velocity.Y += (float)(_speed * delta);

		Position += velocity;

		if (Input.IsActionJustPressed("Grab") && grabbedBlock == null)
			GrabBlock();

		if (Input.IsActionJustPressed("Drop") && grabbedBlock != null)
			DropBlock();
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

	private void GrabBlock()
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
		joint.GlobalPosition = grabbedBlock.GlobalPosition;
		joint.NodeA = GetPath();
		joint.NodeB = grabbedBlock.GetPath();

		GetParent().AddChild(joint);
	}

	private void DropBlock()
	{
		if (joint != null)
		{
			joint.QueueFree();
			joint = null;
		}
		grabbedBlock = null;
	}
}
