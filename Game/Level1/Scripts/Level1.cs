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
	[Export] private string _settingsScenePath = "res://Menus/Settings/Scenes/Settings.tscn";

	[Export] private string _blockScenePath = "res://Game/Scenes/Block.tscn";
	[Export] private string _longBlockScenePath = "res://Game/Scenes/LongBlock.tscn";

	private Window settingsWindow;
	private PackedScene _clawScene = null;
	private Claw _claw = null;
	public Claw Claw => _claw;
	private TextureRect conveyorBelt;
	private PackedScene _blockScene;
	private PackedScene _longBlockScene;
	private TextureButton blockButton;
	private TextureButton longBlockButton;
	private bool longBlockButtonPressed = false;
	private bool blockButtonPressed = false;

	private bool endLevel = false;
	private float beltTargetPosition = -860; // conveyorBelt target position when all blocks are in play
	private float beltMoveSpeed = 200f; // conveyorBelts speed of movement per frame


	public override void _Ready()
	{
		_claw = CreateClaw();

		Control background = GetTree().Root.FindChild("Background", true, false) as Control;
		conveyorBelt = background.FindChild("ConveyorBelt", true, false) as TextureRect;

		PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
		settingsWindow = (Window)settingsScene.Instantiate();
		AddChild(settingsWindow);
		settingsWindow.Hide();

		_blockScene = ResourceLoader.Load<PackedScene>(_blockScenePath);
		_longBlockScene = ResourceLoader.Load<PackedScene>(_longBlockScenePath);

		blockButton = GetNodeOrNull<TextureButton>("BlockButtons/Block");
		longBlockButton = GetNodeOrNull<TextureButton>("BlockButtons/LongBlock");

		blockButton.Pressed += () => 
		{
			blockButtonPressed = true;
			SpawnBlockButtonPressed(_blockScene, blockButton);
		};

		longBlockButton.Pressed += () => 
		{
			longBlockButtonPressed = true;
			SpawnBlockButtonPressed(_longBlockScene, longBlockButton);
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
		if (BlockScene == null)
		{
			GD.PrintErr("[ERROR] Cannot spawn block, scene not loaded!");
		}

		ClawHead clawHead = GetTree().Root.FindChild("ClawHead", true, false) as ClawHead;
		if (clawHead == null)
		{
			GD.PrintErr("[ERROR] ClawHead not found! Block cannot be spawned.");
			return;
		}

		clawHead.collisionShape.SetDeferred("disabled", true);

		RigidBody2D blockInstance = BlockScene.Instantiate<RigidBody2D>();
		this.AddChild(blockInstance);

		blockInstance.GlobalPosition = clawHead.GlobalPosition + new Godot.Vector2(0, 20);

		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");

		clawHead.GrabBlock();

		button.QueueFree();

		// Check if all buttons are removed
		if (blockButtonPressed && longBlockButtonPressed) endLevel = true;
	}

	public override void _Process(double delta)
	{
		if (conveyorBelt != null && endLevel)
		{
			float step = beltMoveSpeed * (float) delta;

			if (conveyorBelt.Position.X > beltTargetPosition)
			{
				conveyorBelt.Position = new Godot.Vector2(conveyorBelt.Position.X - step, conveyorBelt.Position.Y);
			}
		}
	}

	//public void GameStop()
	//public void GameFinish()
	//{
	//	if  (Score > 30)
	//	{
	//		WinWindow.Popup();
	//	}
	//	if else (Score > 50)
	//	{
	//		Win2Window.Popup();
	//	}
	//	if else (Score > 70)
	//	{
	//		Win3Window.Popup();
	//	}
	//	else
	//	{
	//		LoseWindow.Popup();
	//	}
	//}
}
