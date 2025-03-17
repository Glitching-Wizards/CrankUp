using Godot;
using System;
using System.Numerics;

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
	private PackedScene _blockScene;
	private PackedScene _longBlockScene;
	private TextureButton blockButton;
	private TextureButton longBlockButton;


	public override void _Ready()
	{
		_claw = CreateClaw();

		PackedScene settingsScene = (PackedScene)GD.Load(_settingsScenePath);
    	settingsWindow = (Window)settingsScene.Instantiate();
		AddChild(settingsWindow);
		settingsWindow.Hide();

		_blockScene = ResourceLoader.Load<PackedScene>(_blockScenePath);
		_longBlockScene = ResourceLoader.Load<PackedScene>(_longBlockScenePath);

		blockButton = GetNodeOrNull<TextureButton>("BlockButtons/Block");
		longBlockButton = GetNodeOrNull<TextureButton>("BlockButtons/LongBlock");

		blockButton.Pressed += () => SpawnBlockButtonPressed(_blockScene, blockButton);
		longBlockButton.Pressed += () => SpawnBlockButtonPressed(_longBlockScene, longBlockButton);
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

		RigidBody2D blockInstance = BlockScene.Instantiate<RigidBody2D>();
		this.AddChild(blockInstance);

		blockInstance.GlobalPosition = clawHead.GlobalPosition + new Vector2(0, 20);

		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");

		clawHead.GrabBlock();

		button.QueueFree();
	}
	public override void _Process(double delta)
	{
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
