using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CrankUp;
public partial class Level1 : Node2D
{
	private static Level1 _current = null;
	public static Level1 Current
	{
		get { return _current; }
	}

	[Export] private string _clawScenePath = "res://Game/Scenes/Claw.tscn";
	[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Pause.tscn";

	[Export] private string _blockScenePath = "res://Game/Scenes/Block.tscn";
	[Export] private string _containerYellowLScenePath = "res://Game/Scenes/ContainerYellowL.tscn";
	[Export] private string _containerBlueLScenePath = "res://Game/Scenes/ContainerBlueL.tscn";
	[Export] private string _containerRedScenePath = "res://Game/Scenes/ContainerRed.tscn";
	[Export] private string _containerYellowScenePath = "res://Game/Scenes/ContainerYellow.tscn";

	private Window settingsWindow;
	private PackedScene _clawScene = null;
	private Claw _claw = null;
	public Claw Claw => _claw;
	private ClawHead clawHead;

	private TextureRect conveyorBelt;
	private PackedScene _blockScene;
	private PackedScene _containerYellowLScene;
	private PackedScene _containerBlueLScene;
	private PackedScene _containerRedScene;
	private PackedScene _containerYellowScene;

	private TextureButton blockButton;
	private TextureButton block2Button;
	private TextureButton containerYellowLButton;
	private TextureButton containerBlueLButton;
	private TextureButton containerRedButton;
	private TextureButton containerYellowButton;

	private bool blockButtonPressed = false;
	private bool block2ButtonPressed = false;
	private bool containerYellowLButtonPressed = false;
	private bool containerBlueLButtonPressed = false;
	private bool containerRedButtonPressed = false;
	private bool containerYellowButtonPressed = false;

	private bool startLevel = true;
	private bool endLevel = false;
	private float beltTargetPositionStart = -540;
	private float beltTargetPositionEnd = 460; // ConveyorBelt target position when all blocks are in play
	private float beltMoveSpeed = 200f; // ConveyorBelts speed of movement per frame


	public override void _Ready()
	{
		_claw = CreateClaw();

		clawHead = GetTree().Root.FindChild("ClawHead", true, false) as ClawHead;
		if (clawHead == null)
		{
			GD.PrintErr("[ERROR] ClawHead not found! Block cannot be spawned.");
			return;
		}

		// tutorial


		conveyorBelt = GetNodeOrNull<TextureRect>("ConveyorBelt");

		PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
		settingsWindow = (Window)settingsScene.Instantiate();
		AddChild(settingsWindow);
		settingsWindow.Hide();

		//työn alla
		Pause pause = GetTree().Root.GetNode<Pause>("res://Menus/Settings/Scripts/Pause");
		if (pause != null)
		{
			pause.TogglePause();
		}


		_blockScene = ResourceLoader.Load<PackedScene>(_blockScenePath);
		_containerYellowLScene = ResourceLoader.Load<PackedScene>(_containerYellowLScenePath);
		_containerBlueLScene = ResourceLoader.Load<PackedScene>(_containerBlueLScenePath);
		_containerRedScene = ResourceLoader.Load<PackedScene>(_containerRedScenePath);
		_containerYellowScene = ResourceLoader.Load<PackedScene>(_containerYellowScenePath);

		blockButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/Block");
		block2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/Block2");
		containerYellowLButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerYellowL");
		containerBlueLButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBlueL");
		containerRedButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerRed");
		containerYellowButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerYellow");


		blockButton.Pressed += () =>
		{
			blockButtonPressed = true;
			SpawnBlockButtonPressed(_blockScene, blockButton);
		};

		block2Button.Pressed += () =>
		{
			block2ButtonPressed = true;
			SpawnBlockButtonPressed(_blockScene, block2Button);
		};

		containerYellowLButton.Pressed += () =>
		{
			containerYellowLButtonPressed = true;
			SpawnBlockButtonPressed(_containerYellowLScene, containerYellowLButton);
		};

		containerBlueLButton.Pressed += () =>
		{
			containerBlueLButtonPressed = true;
			SpawnBlockButtonPressed(_containerBlueLScene, containerBlueLButton);
		};

		containerRedButton.Pressed += () =>
		{
			containerRedButtonPressed = true;
			SpawnBlockButtonPressed(_containerRedScene, containerRedButton);
		};

		containerYellowButton.Pressed += () =>
		{
			containerYellowButtonPressed = true;
			SpawnBlockButtonPressed(_containerYellowScene, containerYellowButton);
		};
	}

	public Level1()
	{
		_current = this;
	}

	private Claw CreateClaw()
	{
		if (_clawScene == null)
		{
			_clawScene = ResourceLoader.Load<PackedScene>(_clawScenePath);
			if (_clawScene == null) // Check if the scene is loaded correctly
			{
				GD.PrintErr("Claw scene not found!");
				return null;
			}
		}
		return _clawScene.Instantiate<Claw>(); // Instantiate Claw
	}

	private async void SpawnBlockButtonPressed(PackedScene BlockScene, TextureButton button)
	{
		if (clawHead.grabbedBlock != null) return;

		if (BlockScene == null)
		{
			GD.PrintErr("[ERROR] Cannot spawn block, scene not loaded!");
		}

		clawHead.collisionShape.SetDeferred("disabled", true);

		RigidBody2D blockInstance = BlockScene.Instantiate<RigidBody2D>();
		this.AddChild(blockInstance);

		blockInstance.GlobalPosition = clawHead.GlobalPosition + new Godot.Vector2(0, 20);

		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");

		clawHead.GrabBlock();

		button.QueueFree();

		// Check if all buttons are pressed
		if (blockButtonPressed && block2ButtonPressed && containerYellowLButtonPressed && containerBlueLButtonPressed && containerRedButtonPressed && containerYellowButtonPressed) endLevel = true;
	}

	public override void _Process(double delta)
	{
		if (conveyorBelt != null && startLevel)
		{
			float step = beltMoveSpeed * (float)delta;

			if (conveyorBelt.Position.X < beltTargetPositionStart)
			{
				conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X + step, conveyorBelt.Position.Y);
			}
		}

		if (conveyorBelt != null && endLevel)
		{
			float step = beltMoveSpeed * (float)delta;

			if (conveyorBelt.Position.X < beltTargetPositionEnd)
			{
				conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X + step, conveyorBelt.Position.Y);
			}
		}
	}
}