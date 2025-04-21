using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CrankUp;
public partial class Level1 : Node2D
{
	private static Level1 _current = null;
	public static Level1 Current => _current;

	[Export] private string _clawScenePath = "res://Game/Scenes/Claw.tscn";
	[Export] private string _pauseScenePath = "res://Menus/Settings/Scenes/Pause.tscn";
	[Export] private string _blockScenePath = "res://Game/Scenes/Block.tscn";
	[Export] private string _containerYellowLScenePath = "res://Game/Scenes/ContainerYellowL.tscn";
	[Export] private string _containerBlueLScenePath = "res://Game/Scenes/ContainerBlueL.tscn";
	[Export] private string _containerRedScenePath = "res://Game/Scenes/ContainerRed.tscn";
	[Export] private string _containerYellowScenePath = "res://Game/Scenes/ContainerYellow.tscn";
	[Export] private AudioStream levelMusic;
	[Export] private AudioStream conveyorBeltSound;

	private Window pauseWindow;

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
	private TextureButton containerYellowLButton;
	private TextureButton containerBlueLButton;
	private TextureButton containerRedButton;
	private TextureButton containerYellowButton;

	private bool blockButtonPressed = false;
	private bool containerYellowLButtonPressed = false;
	private bool containerBlueLButtonPressed = false;
	private bool containerRedButtonPressed = false;
	private bool containerYellowButtonPressed = false;

	private bool startLevel = true;
	private bool endLevel = false;
	private bool beltSoundPlayed = false;
	private float beltTargetPositionStart = -540;
	private float beltTargetPositionEnd = 460;
	private float beltMoveSpeed = 200f;
	private bool tutorialCompleted = false;

	public override async void _Ready()
	{
		_current = this;

		_claw = CreateClaw();
		AudioManager.PlayMusic(levelMusic);

		clawHead = GetTree().Root.FindChild("ClawHead", true, false) as ClawHead;
		if (clawHead == null)
		{
			GD.PrintErr("[ERROR] ClawHead not found! Block cannot be spawned.");
			return;
		}

		conveyorBelt = GetNodeOrNull<TextureRect>("ConveyorBelt");

		_blockScene = ResourceLoader.Load<PackedScene>(_blockScenePath);
		_containerYellowLScene = ResourceLoader.Load<PackedScene>(_containerYellowLScenePath);
		_containerBlueLScene = ResourceLoader.Load<PackedScene>(_containerBlueLScenePath);
		_containerRedScene = ResourceLoader.Load<PackedScene>(_containerRedScenePath);
		_containerYellowScene = ResourceLoader.Load<PackedScene>(_containerYellowScenePath);

		blockButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/Block");
		containerYellowLButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerYellowL");
		containerBlueLButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerBlueL");
		containerRedButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerRed");
		containerYellowButton = GetNodeOrNull<TextureButton>("ConveyorBelt/BlockButtons/ContainerYellow");

		// Wait one frame to ensure all nodes are fully loaded
		await ToSignal(GetTree(), "idle_frame");

		// Use TextureButton for skip since that's what it is in the scene
		TextureButton skipButton = GetNodeOrNull<TextureButton>("Tutorial/SkipButton");
		Button lastTutorialButton = GetNodeOrNull<Button>("Tutorial/Tutorial9/Button");

		if (skipButton != null)
		{
			skipButton.Pressed += OnTutorialComplete;
			GD.Print("[Level1] SkipButton connected.");
		}
		else
			GD.PrintErr("[Level1] SkipButton not found!");

		if (lastTutorialButton != null)
		{
			lastTutorialButton.Pressed += OnTutorialComplete;
			GD.Print("[Level1] Last tutorial button connected.");
		}
		else
			GD.PrintErr("[Level1] Last tutorial button not found!");

		blockButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			blockButtonPressed = true;
			SpawnBlockButtonPressed(_blockScene, blockButton);
		};

		containerYellowLButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerYellowLButtonPressed = true;
			SpawnBlockButtonPressed(_containerYellowLScene, containerYellowLButton);
		};

		containerBlueLButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerBlueLButtonPressed = true;
			SpawnBlockButtonPressed(_containerBlueLScene, containerBlueLButton);
		};

		containerRedButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerRedButtonPressed = true;
			SpawnBlockButtonPressed(_containerRedScene, containerRedButton);
		};

		containerYellowButton.Pressed += () =>
		{
			if (clawHead.grabbedBlock != null) return;
			containerYellowButtonPressed = true;
			SpawnBlockButtonPressed(_containerYellowScene, containerYellowButton);
		};
	}

	private void OnTutorialComplete()
	{
		if (tutorialCompleted)
			return;

		tutorialCompleted = true;

		CanvasItem tutorial = GetNodeOrNull("Tutorial") as CanvasItem;
		if (tutorial != null)
			tutorial.Visible = false;

		var controlsLeftUi = GetTree().Root.FindChild("ControlsLeftUi", true, false) as ControlsLeftUi;
		if (controlsLeftUi != null)
		{
			controlsLeftUi.StartTimer();
			GD.Print("[Level1] Tutorial complete. Timer started.");
		}
		else
		{
			GD.PrintErr("[Level1] ControlsLeftUi not found.");
		}
	}

	private Claw CreateClaw()
	{
		if (_clawScene == null)
		{
			_clawScene = ResourceLoader.Load<PackedScene>(_clawScenePath);
			if (_clawScene == null)
			{
				GD.PrintErr("Claw scene not found!");
				return null;
			}
		}
		return _clawScene.Instantiate<Claw>();
	}

	private async void SpawnBlockButtonPressed(PackedScene BlockScene, TextureButton button)
	{
		if (BlockScene == null)
		{
			GD.PrintErr("[ERROR] Cannot spawn block, scene not loaded!");
		}

		clawHead.GlobalPosition = new Godot.Vector2(clawHead.GlobalPosition.X, -291);
		clawHead.collisionShape.SetDeferred("disabled", true);

		Block blockInstance = BlockScene.Instantiate<Block>();
		this.AddChild(blockInstance);
		blockInstance.GlobalPosition = clawHead.GlobalPosition + new Godot.Vector2(0, 20);

		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");

		clawHead.GrabBlock();
		button.QueueFree();

		if (blockButtonPressed && containerYellowLButtonPressed && containerBlueLButtonPressed && containerRedButtonPressed && containerYellowButtonPressed)
		{
			endLevel = true;
			beltSoundPlayed = false;
		}
	}
	/// <summary>
	/// This fuction moves the conveyer belt to the right when
	/// the level starts and when all blocks are in play. It also
	/// contains the sound of the conveyer belt.
	/// The sound is played once when the level starts and
	/// when all blocks are in play.
	/// </summary>
	/// <param name="delta"></param>
	public override void _Process(double delta)
	{
		float step = beltMoveSpeed * (float)delta;

		if (conveyorBelt != null && startLevel)
		{
			if (!beltSoundPlayed)
			{
				AudioManager.PlayConveyorSound(conveyorBeltSound);
				beltSoundPlayed = true;
			}

			if (conveyorBelt.Position.X < beltTargetPositionStart)
			{
				conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X + step, conveyorBelt.Position.Y);
			}
		}

		if (conveyorBelt != null && endLevel)
		{
			if (!beltSoundPlayed)
			{
				AudioManager.PlayConveyorSound(conveyorBeltSound);
				beltSoundPlayed = true;
			}

			if (conveyorBelt.Position.X < beltTargetPositionEnd)
			{
				conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X + step, conveyorBelt.Position.Y);
			}
		}
	}
}
