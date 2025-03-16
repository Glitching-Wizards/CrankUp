using System.Linq;
using Godot;

public partial class PlacementArea : Area2D
{
	private CollisionShape2D collisionShape;
	private Label scoreLabel;
	private float totalAreaSize = 0f;
	private float filledAreaSize = 0f;

	public override void _Ready()
	{
		collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
		scoreLabel = GetNodeOrNull<Label>("ScoreLabel");

		if (collisionShape == null)
		{
			GD.PrintErr("Error: CollisionShape2D not found in PlacementArea!");
			return;
		}

		if (scoreLabel == null)
		{
			GD.PrintErr("Error: ScoreLabel not found in PlacementArea!");
			return;
		}

		totalAreaSize = CalculateShapeArea(collisionShape.Shape);
		GD.Print($"Placement Area Size: {totalAreaSize}");
	}

	private float CalculateShapeArea(Shape2D shape)
	{
		if (shape is RectangleShape2D rect)
		{
			return rect.Size.X * rect.Size.Y;
		}
		else
		{
			GD.PrintErr("Unsupported shape type for PlacementArea!");
			return 0f;
		}
	}
	private void _process(float delta)
	{
		RecalculateFilledArea();
	}

	private void RecalculateFilledArea()
	{
		filledAreaSize = 0f;

		foreach (var body in GetOverlappingBodies().OfType<RigidBody2D>())
		{
			var blockCollision = body.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
			if (blockCollision == null) continue;

			float blockArea = CalculateShapeArea(blockCollision.Shape);
			filledAreaSize += blockArea;
		}

		filledAreaSize = Mathf.Clamp(filledAreaSize, 0, totalAreaSize);
		UpdateScore();
	}

	private void UpdateScore()
	{
		float percentageFilled = (filledAreaSize / totalAreaSize) * 100f;
		GD.Print($"[DEBUG] Updated Score: {percentageFilled:F2}%");
		scoreLabel.Text = $"Score: {percentageFilled:F2}%";
	}
}