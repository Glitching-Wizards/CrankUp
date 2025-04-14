using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CrankUp;
public partial class Level3 : Node2D
{
	private Level3 _current = null;
	public Level3 Current
	{
		get { return _current; }
	}

	[Export] private string _clawScenePath = "res://Game/Scenes/Claw.tscn";
	[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";

	[Export] private string _blockScenePath = "res://Game/Scenes/BlockSmall.tscn";
	[Export] private string _containerRedScenePath = "res://Game/Scenes/ContainerRedSmall.tscn";
	[Export] private string _containerBlueLScenePath = "res://Game/Scenes/ContainerBlueLSmall.tscn";
	[Export] private string _containerBlueLInvertedScenePath = "res://Game/Scenes/ContainerBlueLSmallInverted.tscn";
	[Export] private string _containerGreenLScenePath = "res://Game/Scenes/ContainerGreenLSmall.tscn";
	[Export] private string _containerYellowLScenePath = "res://Game/Scenes/ContainerYellowLSmall.tscn";
	[Export] private string _containerYellowLInvertedScenePath = "res://Game/Scenes/ContainerYellowLSmallInverted.tscn";
	[Export] private string _containerYellowScenePath = "res://Game/Scenes/ContainerYellowSmall.tscn";
	[Export] private string _containerGreenScenePath = "res://Game/Scenes/ContainerGreenSmall.tscn";
	[Export] private string _containerRedLScenePath = "res://Game/Scenes/ContainerRedLSmall.tscn";

	[Export] private AudioStream levelMusic;


	private Window settingsWindow;
	private PackedScene _clawScene = null;
	private Claw _claw = null;
	public Claw Claw => _claw;
	private ClawHead clawHead;

	private TextureRect conveyorBelt;
	private PackedScene _blockScene;
	private PackedScene _containerRedScene;
	private PackedScene _containerYellowScene;
	private PackedScene _containerGreenScene;
	private PackedScene _containerGreenLScene;
	private PackedScene _containerYellowLScene;
	private PackedScene _containerRedLScene;
	private PackedScene _containerBlueLScene;
	private PackedScene _containerBlueLInvertedScene;
	private PackedScene _containerYellowLInvertedScene;

	private TextureButton blockButton;
	private TextureButton containerRedButton;
	private TextureButton containerRed2Button;
	private TextureButton containerYellowButton;
	private TextureButton containerGreenButton;
	private TextureButton containerGreenLButton;
	private TextureButton containerBlueLButton;
	private TextureButton containerBlueLInvertedButton;
	private TextureButton containerYellowLButton;
	private TextureButton containerYellowLInvertedButton;
	private TextureButton containerRedLButton;

	private bool blockButtonPressed = false;
	private bool containerRedButtonPressed = false;
	private bool containerRed2ButtonPressed = false;
	private bool containerYellowButtonPressed = false;
	private bool containerGreenButtonPressed = false;
	private bool containerGreenLButtonPressed = false;
	private bool containerBlueLButtonPressed = false;
	private bool containerBlueLInvertedButtonPressed = false;
	private bool containerYellowLButtonPressed = false;
	private bool containerYellowLInvertedButtonPressed = false;
	private bool containerRedLButtonPressed = false;

	private bool startLevel = true;
	private bool endLevel = false;
	private bool interim = false;
	private float beltTargetPositionStart = -540;
	private float beltTargetPositionInterim = 460;
	private float beltTargetPositionEnd = 1440; // conveyorBelt target position when all blocks are in play
	private float beltMoveSpeed = 200f; // conveyorBelts speed of movement per frame


	public override void _Ready()
	{
		_claw = CreateClaw();
		AudioManager.PlayMusic(levelMusic);

		clawHead = GetTree().Root.FindChild("ClawHead", true, false) as ClawHead;

		conveyorBelt = GetNodeOrNull<TextureRect>("ConveyorBelt");

		PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
		settingsWindow = (Window)settingsScene.Instantiate();
		AddChild(settingsWindow);
		settingsWindow.Hide();

		_blockScene = ResourceLoader.Load<PackedScene>(_blockScenePath);
		_containerRedScene = ResourceLoader.Load<PackedScene>(_containerRedScenePath);
		_containerYellowScene = ResourceLoader.Load<PackedScene>(_containerYellowScenePath);
		_containerGreenScene = ResourceLoader.Load<PackedScene>(_containerGreenScenePath);
		_containerGreenLScene = ResourceLoader.Load<PackedScene>(_containerGreenLScenePath);
		_containerBlueLScene = ResourceLoader.Load<PackedScene>(_containerBlueLScenePath);
		_containerBlueLInvertedScene = ResourceLoader.Load<PackedScene>(_containerBlueLInvertedScenePath);
		_containerYellowLScene = ResourceLoader.Load<PackedScene>(_containerYellowLScenePath);
		_containerYellowLInvertedScene = ResourceLoader.Load<PackedScene>(_containerYellowLInvertedScenePath);
		_containerRedLScene = ResourceLoader.Load<PackedScene>(_containerRedLScenePath);

		blockButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/Block");
		containerRedButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerRed");
		containerRed2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerRed2");
		containerGreenButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerGreen");
		containerYellowButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerYellow");
		containerGreenLButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerGreenL");
		containerYellowLButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerYellowL");
		containerBlueLButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBlueL");
		containerBlueLInvertedButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBlueLInverted");
		containerYellowLInvertedButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerYellowLInverted");
		containerRedLButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerRedL");

		blockButton.Pressed += () =>
		{
			blockButtonPressed = true;
			SpawnBlockButtonPressed(_blockScene, blockButton);
		};

		containerRedButton.Pressed += () =>
		{
			containerRedButtonPressed = true;
			SpawnBlockButtonPressed(_containerRedScene, containerRedButton);
		};

		containerRed2Button.Pressed += () =>
		{
			containerRed2ButtonPressed = true;
			SpawnBlockButtonPressed(_containerRedScene, containerRed2Button);
		};

		containerGreenButton.Pressed += () =>
		{
			containerGreenButtonPressed = true;
			SpawnBlockButtonPressed(_containerGreenScene, containerGreenButton);
		};

		containerYellowButton.Pressed += () =>
		{
			containerYellowButtonPressed = true;
			SpawnBlockButtonPressed(_containerYellowScene, containerYellowButton);
		};

		containerGreenLButton.Pressed += () =>
		{
			containerGreenLButtonPressed = true;
			SpawnBlockButtonPressed(_containerGreenLScene, containerGreenLButton);
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

		containerBlueLInvertedButton.Pressed += () =>
		{
			containerBlueLInvertedButtonPressed = true;
			SpawnBlockButtonPressed(_containerBlueLInvertedScene, containerBlueLInvertedButton);
		};

		containerYellowLInvertedButton.Pressed += () =>
		{
			containerYellowLInvertedButtonPressed = true;
			SpawnBlockButtonPressed(_containerYellowLInvertedScene, containerYellowLInvertedButton);
		};

		containerRedLButton.Pressed += () =>
		{
			containerRedLButtonPressed = true;
			SpawnBlockButtonPressed(_containerRedLScene, containerRedLButton);
		};
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

		// Check if all buttons are removed
		if (blockButtonPressed && containerRedButtonPressed && containerGreenLButtonPressed && containerBlueLInvertedButtonPressed && containerYellowLButtonPressed && containerYellowLInvertedButtonPressed) interim = true;
		if (interim && containerGreenButtonPressed && containerYellowButtonPressed && containerRedLButtonPressed && containerRed2ButtonPressed && containerBlueLButtonPressed) endLevel = true;
	}

	public override void _Process(double delta)
	{
		if (conveyorBelt != null && startLevel)
		{
			float step = beltMoveSpeed * (float) delta;

			if (conveyorBelt.Position.X < beltTargetPositionStart)
			{
				conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X + step, conveyorBelt.Position.Y);
			}
		}

		if (conveyorBelt != null && interim)
		{
			float step = beltMoveSpeed * (float) delta;

			if (conveyorBelt.Position.X < beltTargetPositionInterim)
			{
				conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X + step, conveyorBelt.Position.Y);
			}
		}

		if (conveyorBelt != null && endLevel)
		{
			float step = beltMoveSpeed * (float) delta;

			if (conveyorBelt.Position.X < beltTargetPositionEnd)
			{
				conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X + step, conveyorBelt.Position.Y);
			}
		}
	}
}