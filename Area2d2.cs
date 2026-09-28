using Godot;

public partial class WorldBoundaryArea2D : Area2D
{
    public override void _Ready()
    {
        // Create the CollisionShape2D node
        var collisionShape = new CollisionShape2D();

        // Create the WorldBoundaryShape2D resource
        var worldBoundaryShape = new WorldBoundaryShape2D();

        // Configure the normal and distance (e.g., pointing upwards)
        worldBoundaryShape.Normal = new Vector2(0, -1);
        worldBoundaryShape.Distance = 325f;

        // Assign the shape to the collision node
        collisionShape.Shape = worldBoundaryShape;

        // Add the collision shape as a child of the Area2D
        AddChild(collisionShape);

        // Connect the body_entered signal to detect physics bodies hitting/crossing it
        BodyEntered += OnBodyEntered;
    }

    private void OnNode(Node body) // fallback signature or use PhysicsBody2D
    {
        // Placeholder
    }

    private void OnBodyEntered(Node2D body)
    {
        GD.Print($"Body entered world boundary area: {body.Name}");
    }
}