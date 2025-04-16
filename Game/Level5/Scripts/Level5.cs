using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CrankUp;
public partial class Level5 : Node2D
{
	private Level5 _current = null;
	public Level5 Current
	{
		get { return _current; }
	}

	[Export] private string _clawScenePath = "res://Game/Scenes/Claw.tscn";
	[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";

	[Export] private string _containerZScenePath = "res://Game/Scenes/ContainerBoxZ.tscn";
	[Export] private string _containerZInvertedScenePath = "res://Game/Scenes/ContainerBoxZInverted.tscn";
	[Export] private string _containerBoxScenePath = "res://Game/Scenes/ContainerBox.tscn";
	[Export] private string _cardboardTScenePath = "res://Game/Scenes/ContainerCardboardT.tscn";
	[Export] private string _containerYellowScenePath = "res://Game/Scenes/ContainerYellowSmall.tscn";
	[Export] private string _containerBlueLScenePath = "res://Game/Scenes/ContainerBlueLSmall.tscn";
	[Export] private string _containerBlueLInvertedScenePath = "res://Game/Scenes/ContainerBlueLSmallInverted.tscn";
	[Export] private string _containerYellowLInvertedScenePath = "res://Game/Scenes/ContainerYellowLSmallInverted.tscn";

	[Export] private AudioStream levelMusic;
	[Export] private AudioStream conveyorBeltSound;


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
	private PackedScene _containerYellowScene;
	private PackedScene _containerBlueLScene;
	private PackedScene _containerBlueLInvertedScene;
	private PackedScene _containerYellowLInvertedScene;

	private TextureButton containerZButton;
	private TextureButton containerZ2Button;
	private TextureButton containerZInvertedButton;
	private TextureButton containerZInverted2Button;
	private TextureButton cardboardTButton;
	private TextureButton cardboardT2Button;
	private TextureButton containerYellowButton;
	private TextureButton containerBlueLButton;
	private TextureButton containerBlueLInvertedButton;
	private TextureButton containerYellowLInvertedButton;
	private TextureButton containerBoxButton;
	private TextureButton containerBox2Button;

	private bool containerZButtonPressed = false;
	private bool containerZ2ButtonPressed = false;
	private bool containerZInvertedButtonPressed = false;
	private bool containerZInverted2ButtonPressed = false;
	private bool cardboardTButtonPressed = false;
	private bool cardboardT2ButtonPressed = false;
	private bool containerYellowButtonPressed = false;
	private bool containerBlueLButtonPressed = false;
	private bool containerBlueLInvertedButtonPressed = false;
	private bool containerYellowLInvertedButtonPressed = false;
	private bool containerBoxButtonPressed = false;
	private bool containerBox2ButtonPressed = false;

	private bool startLevel = true;
	private bool endLevel = false;
	private bool beltSoundPlayed = false;
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
		_containerZInvertedScene = ResourceLoader.Load<PackedScene>(_containerZInvertedScenePath);
		_containerYellowScene = ResourceLoader.Load<PackedScene>(_containerYellowScenePath);
		_containerBoxScene = ResourceLoader.Load<PackedScene>(_containerBoxScenePath);
		_containerBlueLScene = ResourceLoader.Load<PackedScene>(_containerBlueLScenePath);
		_containerBlueLInvertedScene = ResourceLoader.Load<PackedScene>(_containerBlueLInvertedScenePath);
		_containerYellowLInvertedScene = ResourceLoader.Load<PackedScene>(_containerYellowLInvertedScenePath);
		_cardboardTScene = ResourceLoader.Load<PackedScene>(_cardboardTScenePath);

		containerZButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZ");
		containerZ2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZ2");
		containerZInvertedButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZInverted");
		containerYellowButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerYellow");
		containerBlueLButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBlueL");
		containerBlueLInvertedButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBlueLInverted");
		containerYellowLInvertedButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerYellowLInverted");
		cardboardTButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/CardboardT");
		cardboardT2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/CardboardT2");
		containerBoxButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBox");
		containerBox2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBox2");
		containerZInverted2Button = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerZInverted2");

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

		containerZInvertedButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerZInvertedButtonPressed = true;
			SpawnBlockButtonPressed(_containerZInvertedScene, containerZInvertedButton);
		};

		containerYellowButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerYellowButtonPressed = true;
			SpawnBlockButtonPressed(_containerYellowScene, containerYellowButton);
		};

		containerBlueLButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerBlueLButtonPressed = true;
			SpawnBlockButtonPressed(_containerBlueLScene, containerBlueLButton);
		};

		containerBlueLInvertedButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerBlueLInvertedButtonPressed = true;
			SpawnBlockButtonPressed(_containerBlueLInvertedScene, containerBlueLInvertedButton);
		};

		containerYellowLInvertedButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerYellowLInvertedButtonPressed = true;
			SpawnBlockButtonPressed(_containerYellowLInvertedScene, containerYellowLInvertedButton);
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

		containerZInverted2Button.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerZInverted2ButtonPressed = true;
			SpawnBlockButtonPressed(_containerZInvertedScene, containerZInverted2Button);
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
		if (containerYellowButtonPressed && containerBlueLButtonPressed && containerZButtonPressed && containerZInvertedButtonPressed && containerBoxButtonPressed && cardboardTButtonPressed)
		{
			interim = true;
			startLevel = false;
			beltSoundPlayed = false; // Reset the sound of conveyerBelt
		}
		if (interim && containerBlueLInvertedButtonPressed && containerYellowLInvertedButtonPressed && cardboardT2ButtonPressed && containerBox2ButtonPressed && containerZInverted2ButtonPressed)
		{
			endLevel = true;
			interim = false;
			beltSoundPlayed = false; // Reset the sound of conveyerBelt
		}
	}

	/// <summary>
	/// This fuction moves the conveyer belt to the right when
	/// the level starts and when all blocks are in play. It also
	/// contains the sound of the conveyer belt.
	/// The sound is played once when the level start and
	/// when all blocks are in play.
	/// </summary>
	/// <param name="delta"></param>
	public override void _Process(double delta)
	{
		float step = beltMoveSpeed * (float)delta;

		if (conveyorBelt == null) return;

		if (startLevel && conveyorBelt.Position.X < beltTargetPositionStart)
		{
			if (!beltSoundPlayed)
			{
				AudioManager.PlayConveyorSound(conveyorBeltSound);
				beltSoundPlayed = true;
			}

			conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X + step, conveyorBelt.Position.Y);
		}
		else if (interim && conveyorBelt.Position.X < beltTargetPositionInterim)
		{
			if (!beltSoundPlayed)
			{
				AudioManager.PlayConveyorSound(conveyorBeltSound);
				beltSoundPlayed = true;
			}

			conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X + step, conveyorBelt.Position.Y);
		}
		else if (endLevel && conveyorBelt.Position.X < beltTargetPositionEnd)
		{
			if (!beltSoundPlayed)
			{
				AudioManager.PlayConveyorSound(conveyorBeltSound);
				beltSoundPlayed = true;
			}

			conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X + step, conveyorBelt.Position.Y);
		}
	}
}