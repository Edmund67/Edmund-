using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 400.0f;
    public const float JumpVelocity = -400.0f;

    [Export] public AnimatedSprite2D Sprite;
    
    // 1. Add an Export variable for your Area2D hitbox/hurtbox
    [Export] public Area2D CollisionDetector; 

    private bool _isAttacking = false;

    public override void _Ready()
    {
        Sprite.AnimationFinished += OnAnimationFinished;

        // 2. Connect the body entered signal to our custom method
        if (CollisionDetector != null)
        {
            CollisionDetector.BodyEntered += OnBodyEntered;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        if (Input.IsActionJustPressed("ui_attack") && !_isAttacking) // Fixed: common input map naming
        {
            _isAttacking = true;
            Sprite.Play("Combo 1");
        }

        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
            if (!_isAttacking)
            {
                Sprite.Play("Run");
            }
            Sprite.FlipH = direction.X < 0;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            if (!_isAttacking)
            {
                Sprite.Play("Idle");
            }
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    // 3. This method runs when something hits the player's Area2D
    private void OnBodyEntered(Node2D body)
    {
        // Check if the thing we hit is named "Slime" or belongs to a Slime class
        if (body.Name.ToString().Contains("Enemy") || body is Slime) 
        {
            GD.Print("Player hit a Enemy! Player dies.");
            Die();
        }
    }

    private void Die()
    {
        // Handle player death here (e.g., play death animation, reload scene)
        QueueFree(); 
    }

    private void OnAnimationFinished()
    {
        if (Sprite.Animation == "Combo 1")
        {
            _isAttacking = false;
        }
    }
}
