using Godot;
using System.Linq;

public partial class PlacementArea : Area2D
{
	private CollisionPolygon2D collisionPolygon;
	private Label scoreLabel;
	private float totalAreaSize = 0f;
	private float filledAreaSize = 0f;

	public override void _Ready()
	{
		collisionPolygon = GetNodeOrNull<CollisionPolygon2D>("CollisionPolygon2D");
		scoreLabel = GetNodeOrNull<Label>("ScoreLabel");

		if (collisionPolygon == null)
		{
			GD.PrintErr("Error: CollisionPolygon2D not found in PlacementArea!");
			return;
		}

		if (scoreLabel == null)
		{
			GD.PrintErr("Error: ScoreLabel not found in PlacementArea!");
			return;
		}

		totalAreaSize = CalculatePolygonArea(collisionPolygon.Polygon);
		GD.Print($"Placement Area Size: {totalAreaSize}");
	}

	private float CalculatePolygonArea(Vector2[] points)
	{
		if (points.Length < 3) return 0f; // Not a valid polygon

		float area = 0f;
		int j = points.Length - 1;

		for (int i = 0; i < points.Length; i++)
		{
			area += (points[j].X + points[i].X) * (points[j].Y - points[i].Y);
			j = i;
		}

		return Mathf.Abs(area / 2f);
	}

	private void _on_body_entered(Node body)
	{
		GD.Print($"Body entered: {body.Name}");

		if (body is RigidBody2D block)
		{
			// Estimate block area (simplified: using bounding box)
			GD.Print("A block has entered the placement area!");
			var blockCollision = block.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
			if (blockCollision != null && blockCollision.Shape is RectangleShape2D rectShape)
			{
				filledAreaSize += rectShape.Size.X * rectShape.Size.Y;
			}

			float percentageFilled = (filledAreaSize / totalAreaSize) * 100f;
			scoreLabel.Text = $"Score: {percentageFilled:F2}%";
			GD.Print($"Updated Score: {percentageFilled:F2}%");
		}
	}
}
