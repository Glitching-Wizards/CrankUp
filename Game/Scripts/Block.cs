using Godot;
using System;

public partial class Block : RigidBody2D
{
	[Export] private int maxHealth = 3;
	private int currentHealth;
	private Sprite2D sprite;
	private PinJoint2D joint; 

	[Export] private Texture2D healthy;
	[Export] private Texture2D damaged;
	[Export] private Texture2D broken;

	public override void _Ready()
	{
		AddToGroup("blocks");

		joint = GetParent().GetNodeOrNull<PinJoint2D>("PinJoint2D");

		sprite = GetNode<Sprite2D>("Sprite");
		currentHealth = maxHealth;
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		if (currentHealth == 3)
			sprite.Texture = healthy;
		else if (currentHealth == 2)
			sprite.Texture = damaged;
		else if (currentHealth == 1)
			sprite.Texture = broken;
	}

	private void TakeDamage()
	{
		currentHealth--;
		UpdateSprite();
	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		Vector2 LinearVelocity = state.LinearVelocity;

		if (LinearVelocity.Length() > 300f)
		{
			TakeDamage();
		}
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Drop") && joint != null)
		{
			joint.QueueFree();
			joint = null;
		}
	}
}
