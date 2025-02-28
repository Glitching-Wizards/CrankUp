using Godot;

public partial class Block : RigidBody2D
{
	private PinJoint2D joint; 

	public override void _Ready()
	{
		AddToGroup("blocks");
		joint = GetParent().GetNodeOrNull<PinJoint2D>("PinJoint2D");
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Drop") && joint != null)
		{
			joint.QueueFree();
			joint = null;
		}
	}
}
