using Godot;
using System;


public partial class Fish : RigidBody2D
{
    enum State
    {
        Alive,
		Dead
    }

	[Export]
    public float value = 1;
	[Export]
	public int multiplier = 1;
	[Export]
	public bool isNegative = false;
	[Export]
	public Path2D path = null;

    public bool flip = false;

    private AnimatedSprite2D sprite;
	private CollisionShape2D hurtBox;
	private State state;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		hurtBox = GetNode<CollisionShape2D>("HurtBox");
		state = State.Alive;

		if (LinearVelocity.X == 0)
		{
			LinearVelocity = new Vector2((float)GD.RandRange(150.0, 250.0) * (flip ? 1 : -1), 0);
		}
		sprite.FlipH = flip;
		if (flip)
		{
			Flip(hurtBox);
        }

        sprite.Animation = "alive";
        sprite.Play();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
    }

    private void DelayedDispose()
	{
		GetTree().CreateTimer(1).Timeout += QueueFree;
	}

	protected void Flip(Node2D node)
	{
        Vector2 unflipped = node.Position;
        node.Position = new Vector2(unflipped.X * -1, unflipped.Y);
    }

	public void Catch()
	{
        hurtBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    public void Trigger()
	{
		Kill();
	}

	public void Kill()
	{
		if (state == State.Dead) return;
		state = State.Dead;
        LinearVelocity = new Vector2(0, 0);
        GravityScale = 1;
		sprite.Animation = "dead";
    }
}


