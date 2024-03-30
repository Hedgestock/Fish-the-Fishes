using Godot;
using Godot.Collections;
using System;
using System.Linq;


public partial class Fish : RigidBody2D
{
    protected enum State
    {
        Alive,
		Dead,
        Fished
    }

    [ExportGroup("Scoring")]
    [Export]
    public float value = 1;
	[Export]
	public int multiplier = 1;
	[Export]
	public bool isNegative = false;

    [ExportGroup("Behaviour")]
    [Export]
    public float minSpeed = 150;
    [Export]
    public float maxSpeed = 250;

    public bool flip = false;
    public float actualSpeed = 0;

	protected Node2D flipacious;
	protected Array<CollisionShape2D> hurtBoxes;
    protected AnimatedSprite2D sprite;

    protected State state;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		flipacious = GetNode<Node2D>("Flipacious");
        hurtBoxes = (Array<CollisionShape2D>) GetHurtboxes();
        sprite = flipacious.GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        state = State.Alive;

        if (actualSpeed == 0)
        {
            actualSpeed = (float) GD.RandRange(minSpeed, maxSpeed) * (flip ? 1 : -1);
		}

        LinearVelocity = new Vector2(actualSpeed, 0);

        if (flip)
        {
            flipacious.Scale = new Vector2(-1, 1);
            foreach (CollisionShape2D hurtBox in hurtBoxes)
            {
                FlipHurtbox(hurtBox);
            }
        }

        sprite.Animation = "alive";
        sprite.Play();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
    }

    private Variant GetHurtboxes()
    {
        return GetChildren().Where(child => child.GetGroups().Contains("Hurtboxes")).ToArray();
    }

    private void FlipHurtbox(Node2D node)
    {
        Vector2 unflipped = node.Position;
        node.Position = new Vector2(unflipped.X * -1, unflipped.Y);
        node.Scale = new Vector2(-1, 1);
    }
    private void DelayedDispose()
	{
		GetTree().CreateTimer(1).Timeout += QueueFree;
	}

	public void Catch(Vector2 Velocity)
	{
        state = State.Fished;
        GravityScale = 0;
        LinearVelocity = Velocity;
        foreach (CollisionShape2D hurtBox in hurtBoxes)
        {
            hurtBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        }
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


