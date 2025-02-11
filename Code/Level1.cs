using Godot;
using System;

namespace CraneGame;
public partial class Level1 : Node2D
{
	private static Level1 _current = null;
	public static Level1 Current
	{
		get { return _current; }
	}

	[Export] private string _clawScenePath = "res://Levels/claw.tscn";
	private PackedScene _clawScene = null;
	private Claw _claw = null;
	public Claw Claw => _claw;


	public override void _Ready()
	{
		_claw = CreateClaw();
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

	public override void _Process(double delta)
	{
	}
}
