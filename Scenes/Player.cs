using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 350.0f;
    public const float JumpVelocity = -400.0f;
	
    [Export]
    public AnimatedSprite2D Sprite;

    // Track if the player is currently in an attack animation
    private bool _isAttacking = false;

    public override void _Ready()
    {
        // Connect the animation finished signal to reset the attack state
        Sprite.AnimationFinished += OnAnimationFinished;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Trigger Attack (Ensure you map "attack" in Project Settings -> Input Map)
        if (Input.IsActionJustPressed("A") && !_isAttacking)
        {
            _isAttacking = true;
            Sprite.Play("Combo 1");
            // Optional: Stop horizontal movement during attack by setting velocity.X = 0;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        // Get the input direction and handle the movement/deceleration.
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;
			
            // Only play Run if we aren't currently attacking
            if (!_isAttacking)
            {
                Sprite.Play("Run");
            }
			
            // Allow flipping direction even while attacking
            Sprite.FlipH = direction.X < 0;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			
            // Only play Idle if we aren't currently attacking
            if (!_isAttacking)
            {
                Sprite.Play("Idle");
            }
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private void OnAnimationFinished()
    {
        // Reset the attacking flag when the attack animation completes
        if (Sprite.Animation == "Combo 1")
        {
            _isAttacking = false;
        }
    }
}