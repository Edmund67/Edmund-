using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 60.0f;
    [Export] public float Gravity { get; set; } = 900.0f;

    private int _direction = 1;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add gravity
        if (!IsOnFloor())
        {
            velocity.Y += Gravity * (float)delta;
        }

        // Set horizontal movement
        velocity.X = Speed * _direction;

        Velocity = velocity;
        MoveAndSlide();

        // Change direction if hitting a wall
        if (IsOnWall())
        {
            _direction *= -1;
            
            // Flip sprite or scale if needed
            var sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
            if (sprite != null)
            {
                sprite.FlipH = _direction < 0;
            }
        }
    }

    public void KillPlayer(Node2D node)
    {
        if (node is Player)
            GetTree().ReloadCurrentScene();
    }
}