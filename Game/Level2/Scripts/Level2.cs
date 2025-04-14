using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CrankUp;
public partial class Level2 : Node2D
{
	private static Level2 _current = null;
	public static Level2 Current
	{
		get { return _current; }
	}

	[Export] private string _clawScenePath = "res://Game/Scenes/Claw.tscn";
	[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";

	[Export] private string _containerZScenePath = "res://Game/Scenes/ContainerBoxZ.tscn";
	[Export] private string _containerBoxScenePath = "res://Game/Scenes/ContainerBox.tscn";
	[Export] private string _cardboardTScenePath = "res://Game/Scenes/ContainerCardboardT.tscn";

	[Export] private AudioStream levelMusic;


	private Window settingsWindow;
	private PackedScene _clawScene = null;
	private Claw _claw = null;
	public Claw Claw => _claw;
	private ClawHead clawHead;

	private TextureRect conveyorBelt;
	private PackedScene _containerZScene;
	private PackedScene _containerBoxScene;
	private PackedScene _cardboardTScene;

	private TextureButton containerZButton;
	private TextureButton containerZ2Button;
	private TextureButton containerZ3Button;
	private TextureButton containerBoxButton;
	private TextureButton containerBox2Button;
	private TextureButton containerBox3Button;
	private TextureButton containerBox4Button;
	private TextureButton cardboardTButton;
	private TextureButton cardboardT2Button;

	private bool containerZButtonPressed = false;
	private bool containerZ2ButtonPressed = false;
	private bool containerZ3ButtonPressed = false;
	private bool containerBoxButtonPressed = false;
	private bool containerBox2ButtonPressed = false;
	private bool containerBox3ButtonPressed = false;
	private bool containerBox4ButtonPressed = false;
	private bool cardboardTButtonPressed = false;
	private bool cardboardT2ButtonPressed = false;

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

		_containerZScene = ResourceLoader.Load<PackedScene>(_containerZScenePath);
		_containerBoxScene = ResourceLoader.Load<PackedScene>(_containerBoxScenePath);
		_cardboardTScene = ResourceLoader.Load<PackedScene>(_cardboardTScenePath);

		containerZButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZ");
		containerZ2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZ2");
		containerZ3Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZ3");
		containerBoxButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBox");
		containerBox2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBox2");
		containerBox3Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBox3");
		containerBox4Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBox4");
		cardboardTButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/CardboardT");
		cardboardT2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/CardboardT2");

		containerZButton.Pressed += () =>
		{
			containerZButtonPressed = true;
			SpawnBlockButtonPressed(_containerZScene, containerZButton);
		};

		containerZ2Button.Pressed += () =>
		{
			containerZ2ButtonPressed = true;
			SpawnBlockButtonPressed(_containerZScene, containerZ2Button);
		};

		containerZ3Button.Pressed += () =>
		{
			containerZ3ButtonPressed = true;
			SpawnBlockButtonPressed(_containerZScene, containerZ3Button);
		};

		containerBoxButton.Pressed += () =>
		{
			containerBoxButtonPressed = true;
			SpawnBlockButtonPressed(_containerBoxScene, containerBoxButton);
		};

		containerBox2Button.Pressed += () =>
		{
			containerBox2ButtonPressed = true;
			SpawnBlockButtonPressed(_containerBoxScene, containerBox2Button);
		};

		containerBox3Button.Pressed += () =>
		{
			containerBox3ButtonPressed = true;
			SpawnBlockButtonPressed(_containerBoxScene, containerBox3Button);
		};

		containerBox4Button.Pressed += () =>
		{
			containerBox4ButtonPressed = true;
			SpawnBlockButtonPressed(_containerBoxScene, containerBox4Button);
		};

		cardboardTButton.Pressed += () =>
		{
			cardboardTButtonPressed = true;
			SpawnBlockButtonPressed(_cardboardTScene, cardboardTButton);
		};

		cardboardT2Button.Pressed += () =>
		{
			cardboardT2ButtonPressed = true;
			SpawnBlockButtonPressed(_cardboardTScene, cardboardT2Button);
		};
	}

	public Level2()
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

		// Check if all buttons are removed
		if (containerZButtonPressed && containerZ2ButtonPressed && containerZ3ButtonPressed && cardboardTButtonPressed && cardboardT2ButtonPressed && containerBoxButtonPressed) interim = true;
		if (interim && containerBox2ButtonPressed && containerBox3ButtonPressed && containerBox4ButtonPressed) endLevel = true;
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
