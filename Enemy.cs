using Godot;
using System;
public partial class Enemy : CharacterBody2D
{
    [Export] public int MaxHealth { get; set; } = 3;
    private int _currentHealth;
    private bool _isDead = false;

    private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        _currentHealth = MaxHealth;
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        
        // Connect the animation finished signal to handle cleanup after death
        _animatedSprite.AnimationFinished += OnAnimationFinished;
        
        // Play idle by default
        PlayAnimation("Idle");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_isDead) return;

        // Add your movement or AI logic here
        // If standing still:
        PlayAnimation("Idle");
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        PlayAnimation("Die");
        
        // Disable collisions so the dead enemy doesn't interact with the player anymore
        var collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
        if (collisionShape != null)
        {
            collisionShape.SetDeferred("disabled", true);
        }
    }

    private void PlayAnimation(string animName)
    {
        if (_animatedSprite.Animation != animName)
        {
            _animatedSprite.Play(animName);
        }
    }

    private void OnAnimationFinished()
    {
        // Free the enemy node only when the "die" animation completes
        if (_isDead && _animatedSprite.Animation == "die")
        {
            QueueFree();
        }
    }
}