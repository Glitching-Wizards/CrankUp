using System.Linq;
using Godot;

public partial class PlacementArea : Area2D
{
	private CollisionShape2D collisionShape;
	private Label scoreLabel;
	private float totalAreaSize = 0f;
	private float filledAreaSize = 0f;

	public override void _Ready() {
		collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
		scoreLabel = GetNodeOrNull<Label>("ScoreLabel");

		if (collisionShape == null || scoreLabel == null)
		return;

		totalAreaSize = CalculateShapeArea(collisionShape.Shape);
	}

	private float CalculateShapeArea(Shape2D shape) {
		if (shape is RectangleShape2D rect)
		return rect.Size.X * rect.Size.Y;

		return 0f;
	}

	private void _process(float delta) {
		RecalculateFilledArea();
	}

	private void RecalculateFilledArea() {
		filledAreaSize = 0f;
		var placementPolygon = GetCollisionPolygon(collisionShape);

		foreach (var body in GetOverlappingBodies().OfType<RigidBody2D>())
		{
			var blockCollision = body.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
			if (blockCollision == null) continue;

			var blockPolygon = GetCollisionPolygon(blockCollision);
			filledAreaSize += CalculateOverlapArea(placementPolygon, blockPolygon);
		}

		filledAreaSize = Mathf.Clamp(filledAreaSize, 0, totalAreaSize);
		UpdateScore();
	}

	private void UpdateScore() {
		float percentageFilled = (filledAreaSize / totalAreaSize) * 100f;
		if (scoreLabel != null)
			scoreLabel.Text = $"Score: {percentageFilled:F2}%";
	}

	private Vector2[] GetCollisionPolygon(CollisionShape2D collisionShape) {
		if (collisionShape.Shape is RectangleShape2D rect)
		{
			Vector2 size = rect.Size / 2;
			return new Vector2[]
			{
				collisionShape.GlobalPosition + new Vector2(-size.X, -size.Y),
				collisionShape.GlobalPosition + new Vector2(size.X, -size.Y),
				collisionShape.GlobalPosition + new Vector2(size.X, size.Y),
				collisionShape.GlobalPosition + new Vector2(-size.X, size.Y),
			};
		}
		return new Vector2[0];
	}

	private float CalculatePolygonArea(Vector2[] polygon) {
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

	private float CalculateOverlapArea(Vector2[] poly1, Vector2[] poly2) {
		var intersection = Geometry2D.IntersectPolygons(poly1, poly2);
		if (intersection.Any() && intersection[0].Length > 2)
			return CalculatePolygonArea(intersection[0]);

		return 0f;
	}
}