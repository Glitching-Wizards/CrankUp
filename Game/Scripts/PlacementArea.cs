using Godot;
using System.Linq;

namespace CrankUp;
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

		if (collisionPolygon == null || scoreLabel == null)
			return;
		
		if (scoreLabel != null)
			scoreLabel.Visible = true;

		totalAreaSize = CalculatePolygonArea(collisionPolygon.Polygon);
	}

	private void RecalculateFilledArea()
	{
		filledAreaSize = 0f;
		var placementPolygon = collisionPolygon.Polygon;

		foreach (var body in GetOverlappingBodies().OfType<RigidBody2D>())
		{
			var blockCollision = body.GetNodeOrNull<CollisionPolygon2D>("CollisionPolygon2D");
			
			if (blockCollision == null)
			{
				continue;
			}

			var blockPolygon = blockCollision.Polygon;
			float overlap = CalculateOverlapArea(placementPolygon, blockPolygon);

			filledAreaSize += overlap;
		}

		filledAreaSize = Mathf.Clamp(filledAreaSize, 0, totalAreaSize);
		UpdateScore();
	}

	private void UpdateScore()
	{
		float percentageFilled = (filledAreaSize / totalAreaSize) * 100f;
		if (scoreLabel != null)
			scoreLabel.Text = $"Score: {percentageFilled:F2}%";
	}

	private float CalculatePolygonArea(Vector2[] polygon)
	{
		if (polygon.Length < 3) return 0f;
		float area = 0f;
		int j = polygon.Length - 1;

		for (int i = 0; i < polygon.Length; i++)
		{
			area += (polygon[j].X + polygon[i].X) * (polygon[j].Y - polygon[i].Y);
			j = i;
		}
		return Mathf.Abs(area / 2f);
	}

	private float CalculateOverlapArea(Vector2[] poly1, Vector2[] poly2)
	{
		var intersection = Geometry2D.IntersectPolygons(poly1, poly2);
		if (intersection.Any() && intersection[0].Length > 2)
			return CalculatePolygonArea(intersection[0]);

		return 0f;
	}

	public float GetScore()
	{
		if (totalAreaSize <= 0) return 0;
		return (filledAreaSize / totalAreaSize) * 100f;

	}

	public override void _Process(double delta)
	{
		RecalculateFilledArea();
	}
}
