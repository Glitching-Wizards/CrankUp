using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CrankUp;
public partial class Level4 : Node2D
{
	private Level4 _current = null;
	public Level4 Current
	{
		get { return _current; }
	}

	[Export] private string _clawScenePath = "res://Game/Scenes/Claw.tscn";
	[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";

	[Export] private string _containerZScenePath = "res://Game/Scenes/ContainerBoxZ.tscn";
	[Export] private string _containerZInvertedScenePath = "res://Game/Scenes/ContainerBoxZInverted.tscn";
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
	private PackedScene _containerZInvertedScene;
	private PackedScene _containerBoxScene;
	private PackedScene _cardboardTScene;

	private TextureButton containerZButton;
	private TextureButton containerZ2Button;
	private TextureButton containerZ3Button;
	private TextureButton containerZInvertedButton;
	private TextureButton containerZInverted2Button;
	private TextureButton containerZInverted3Button;
	private TextureButton containerBoxButton;
	private TextureButton containerBox2Button;
	private TextureButton containerBox3Button;
	private TextureButton containerBox4Button;
	private TextureButton cardboardTButton;
	private TextureButton cardboardT2Button;
	private TextureButton cardboardT3Button;

	private bool containerZButtonPressed = false;
	private bool containerZ2ButtonPressed = false;
	private bool containerZ3ButtonPressed = false;
	private bool containerZInvertedButtonPressed = false;
	private bool containerZInverted2ButtonPressed = false;
	private bool containerZInverted3ButtonPressed = false;
	private bool containerBoxButtonPressed = false;
	private bool containerBox2ButtonPressed = false;
	private bool containerBox3ButtonPressed = false;
	private bool containerBox4ButtonPressed = false;
	private bool cardboardTButtonPressed = false;
	private bool cardboardT2ButtonPressed = false;
	private bool cardboardT3ButtonPressed = false;

	private bool startLevel = true;
	private bool endLevel = false;
	private bool interim = false;
	private bool interim2 = false;
	private float beltTargetPositionStart = -540;
	private float beltTargetPositionInterim = 460;
	private float beltTargetPositionInterim2 = 800;
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
		_containerZInvertedScene = ResourceLoader.Load<PackedScene>(_containerZInvertedScenePath);
		_containerBoxScene = ResourceLoader.Load<PackedScene>(_containerBoxScenePath);
		_cardboardTScene = ResourceLoader.Load<PackedScene>(_cardboardTScenePath);

		containerZButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZ");
		containerZ2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZ2");
		containerZ3Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZ3");
		containerZInvertedButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZInverted");
		containerZInverted2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZInverted2");
		containerZInverted3Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZInverted3");
		containerBoxButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBox");
		containerBox2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBox2");
		containerBox3Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBox3");
		containerBox4Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBox4");
		cardboardTButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/CardboardT");
		cardboardT2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/CardboardT2");
		cardboardT3Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/CardboardT3");

		containerZButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerZButtonPressed = true;
			SpawnBlockButtonPressed(_containerZScene, containerZButton);
		};

		containerZ2Button.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerZ2ButtonPressed = true;
			SpawnBlockButtonPressed(_containerZScene, containerZ2Button);
		};

		containerZ3Button.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerZ3ButtonPressed = true;
			SpawnBlockButtonPressed(_containerZScene, containerZ3Button);
		};

		containerZInvertedButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerZInvertedButtonPressed = true;
			SpawnBlockButtonPressed(_containerZInvertedScene, containerZInvertedButton);
		};

		containerZInverted2Button.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerZInverted2ButtonPressed = true;
			SpawnBlockButtonPressed(_containerZInvertedScene, containerZInverted2Button);
		};

		containerZInverted3Button.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerZInverted3ButtonPressed = true;
			SpawnBlockButtonPressed(_containerZInvertedScene, containerZInverted3Button);
		};

		containerBoxButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerBoxButtonPressed = true;
			SpawnBlockButtonPressed(_containerBoxScene, containerBoxButton);
		};

		containerBox2Button.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerBox2ButtonPressed = true;
			SpawnBlockButtonPressed(_containerBoxScene, containerBox2Button);
		};

		containerBox3Button.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerBox3ButtonPressed = true;
			SpawnBlockButtonPressed(_containerBoxScene, containerBox3Button);
		};

		containerBox4Button.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerBox4ButtonPressed = true;
			SpawnBlockButtonPressed(_containerBoxScene, containerBox4Button);
		};

		cardboardTButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			cardboardTButtonPressed = true;
			SpawnBlockButtonPressed(_cardboardTScene, cardboardTButton);
		};

		cardboardT2Button.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			cardboardT2ButtonPressed = true;
			SpawnBlockButtonPressed(_cardboardTScene, cardboardT2Button);
		};

		cardboardT3Button.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			cardboardT3ButtonPressed = true;
			SpawnBlockButtonPressed(_cardboardTScene, cardboardT3Button);
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

		clawHead.GlobalPosition = new Godot.Vector2 (clawHead.GlobalPosition.X, -291);

		clawHead.collisionShape.SetDeferred("disabled", true);

		Block blockInstance = BlockScene.Instantiate<Block>();
		this.AddChild(blockInstance);

		blockInstance.GlobalPosition = clawHead.GlobalPosition + new Godot.Vector2(0, 20);

		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");

		clawHead.GrabBlock();

		button.QueueFree();

		// Check if all buttons are removed
		if (containerZButtonPressed && containerZ2ButtonPressed && containerZInvertedButtonPressed && containerZInverted2ButtonPressed && cardboardTButtonPressed && containerBoxButtonPressed) interim = true;
		if (interim && containerBox2ButtonPressed && containerBox3ButtonPressed && containerBox4ButtonPressed && cardboardT2ButtonPressed && cardboardT3ButtonPressed) interim2 = true;
		if (interim2 && containerZ3ButtonPressed && containerZInverted3ButtonPressed) endLevel = true;
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

		if (conveyorBelt != null && interim2)
		{
			float step = beltMoveSpeed * (float) delta;

			if (conveyorBelt.Position.X < beltTargetPositionInterim2)
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