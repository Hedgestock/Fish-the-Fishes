using Godot;
using System;


public partial class Fish : RigidBody2D
{
	public bool flip = false;

    private AnimatedSprite2D sprite;
	private CollisionShape2D hurtBox;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		hurtBox = GetNode<CollisionShape2D>("HurtBox");

        LinearVelocity = new Vector2((float)GD.RandRange(150.0, 250.0) * (flip ? 1 : -1), 0);
		sprite.FlipH = flip;
		if (flip)
		{
			Flip(GetNode<CollisionShape2D>("HurtBox"));
        }

		sprite.Animation = "alive";
        sprite.Play();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
    }

    async private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		GetTree().CreateTimer(1).Timeout += QueueFree;
	}

	protected void Flip(Node2D node)
	{
        Vector2 unflipped = node.Position;
        node.Position = new Vector2(unflipped.X * -1, unflipped.Y);
    }

    public void Trigger()
	{
		Die();
	}

	public void Die()
	{
        hurtBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        LinearVelocity = new Vector2(0, 0);
        GravityScale = 1;
		sprite.Animation = "dead";
    }
}


