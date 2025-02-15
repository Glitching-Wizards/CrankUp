using Godot;

public partial class Rope : CharacterBody2D
{
	private float _speed = 100f;

	public override void _Process(double delta) 
	{
		if (Input.IsActionPressed("Left"))
		{
			Position -= new Vector2((float)(_speed * delta), 0);
		}
		else if (Input.IsActionPressed("Right"))
		{
			Position += new Vector2((float)(_speed * delta), 0);
		}
	}
}
