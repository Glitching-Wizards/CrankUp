using Godot;
using System;

namespace CrankUp;
public partial class Block : RigidBody2D
{
	[Export] private int maxHealth = 3;
	private int currentHealth;
	private Vector2 lastVelocity = Vector2.Zero;
	private float damageCooldown = 0f;
	private Sprite2D sprite;
	private Sprite2D smokeCloud;
	private AnimationPlayer animationPlayer;
	private ClawHead clawHead;
	private PinJoint2D joint;

	[Export] private Texture2D healthy;
	[Export] private Texture2D damaged;
	[Export] private Texture2D broken;
	[Export] private AudioStream hitSound = GD.Load<AudioStream>("res://Audio/SoundEffects/ContainerHit2.mp3");
	[Export] private AudioStream explosionSound = GD.Load<AudioStream>("res://Audio/SoundEffects/Explosion.mp3");

	public override void _Ready()
	{
		AddToGroup("blocks");

		clawHead = GetTree().Root.FindChild("ClawHead", true, false) as ClawHead;

		joint = GetParent().GetNodeOrNull<PinJoint2D>("PinJoint2D");

		smokeCloud = GetNodeOrNull<Sprite2D>("SmokeCloud");
		animationPlayer = GetNodeOrNull<AnimationPlayer>("SmokeCloud/AnimationPlayer");

		sprite = GetNode<Sprite2D>("Sprite");
		currentHealth = maxHealth;
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		if (currentHealth > 2)
			sprite.Texture = healthy;
		else if (currentHealth > 1)
			sprite.Texture = damaged;
		else if (currentHealth > 0)
			sprite.Texture = broken;
	}

	private async void TakeDamage(int damage)
	{
		currentHealth -= damage;
		AudioManager.PlaySound(hitSound);
		UpdateSprite();
		if (currentHealth <= 0)
		{
			AudioManager.PlaySound(explosionSound);
			smokeCloud.Visible = true;
			animationPlayer.Play("SmokeCloudAnimation");

			await ToSignal(GetTree().CreateTimer(0.4f), "timeout");
			sprite.Visible = false;

			await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
			this.QueueFree();

			if (clawHead.grabbedBlock != null)
			{
				clawHead.DropBlock();
			}
		}
	}

	public override void _IntegrateForces(PhysicsDirectBodyState2D state)
	{
		Vector2 currentVelocity = state.LinearVelocity;

		// Estimate the change in velocity
		Vector2 velocityChange = lastVelocity - currentVelocity;
		float impactForce = velocityChange.Length();

		if (impactForce > 500f && damageCooldown <= 0f)
		{
			TakeDamage(2);
			damageCooldown = 0.3f;
		}
		else if (impactForce > 200f && damageCooldown <= 0f)
		{
			TakeDamage(1);
			damageCooldown = 0.3f;
		}

		lastVelocity = currentVelocity;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (damageCooldown > 0)
		{
			damageCooldown -= (float)delta;
		}

		if (Input.IsActionJustPressed("Drop") && joint != null)
		{
			joint.QueueFree();
			joint = null;
		}
	}
}
