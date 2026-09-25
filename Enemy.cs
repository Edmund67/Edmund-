using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    // Export variables allow you to change these values inside the Godot Inspector
    [Export] public float Speed { get; set; } = 150.0f;
    [Export] public int MaxHealth { get; set; } = 100;
    
    private int _currentHealth;
    private Node2D _playerTarget;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _currentHealth = MaxHealth;

        // Safely look for the player in the current scene tree
        // Note: Assumes your player node is named "Player" or belongs to a "player" group
        _playerTarget = GetTree().GetFirstNodeInGroup("player") as Node2D;
    }

    // Called every physics frame. Use this for movement logic.
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (_playerTarget != null)
        {
            // Calculate direction toward the player
            Vector2 direction = (_playerTarget.GlobalPosition - GlobalPosition).Normalized();
            
            // Apply speed to the direction
            velocity = direction * Speed;
            
            // Optional: Flip the sprite/graphics depending on movement direction
            if (direction.X != 0)
            {
                // Assuming you have a Sprite2D child node named "Sprite2D"
                var sprite = GetNode<Sprite2D>("Sprite2D");
                sprite.FlipH = direction.X < 0;
            }
        }
        else
        {
            // Stop moving if no player is found
            velocity = Vector2.Zero;
        }

        Velocity = velocity;
        MoveAndSlide(); // Handles sliding along walls and collision physics automatically
    }

    // Public method that can be called by bullets, spells, or player attacks
    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        GD.Print($"{Name} took {damageAmount} damage. Health remaining: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GD.Print($"{Name} has died.");
        // QueueFree removes the enemy node from the scene tree safely at the end of the frame
        QueueFree(); 
    }
}
