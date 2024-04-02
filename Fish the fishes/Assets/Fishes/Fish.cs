using Godot;
using Godot.Collections;
using System;
using System.Linq;


public partial class Fish : CharacterBody2D
{
    public enum FishState
    {
        Alive,
		Dead,
        Fished
    }

    [ExportGroup("Scoring")]
    [Export]
    public float Value = 1;
	[Export]
	public uint Multiplier = 1;
	[Export]
	public bool IsNegative = false;

    [ExportGroup("Behaviour")]
    [Export]
    public float MinSpeed = 150;
    [Export]
    public float MaxSpeed = 250;
    [Export]
    public float GravityScale = 0;

    public bool Flip = false;
    public float ActualSpeed = 0;
    public FishState State;

    protected AnimatedSprite2D Sprite;


    private Timer DisposeTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        DisposeTimer = GetNode<Timer>("DisposeTimer");
        DisposeTimer.Timeout += QueueFree;

        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        State = FishState.Alive;

        if (ActualSpeed == 0)
        {
            ActualSpeed = (float) GD.RandRange(MinSpeed, MaxSpeed) * (Flip ? -1 : 1);
		}

        Velocity = new Vector2(ActualSpeed, 0);

        if (Flip)
        {
            Scale = new Vector2(-1, 1);
        }

        Sprite.Animation = "alive";
        Sprite.Play();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;
        velocity.Y += (float)delta * GravityScale * (int)ProjectSettings.GetSetting("physics/2d/default_gravity");
        Velocity = velocity;
        MoveAndSlide();
    }

    private void DelayedDispose()
	{
        DisposeTimer.Start();
	}

    private void CancelDispose()
    {
        DisposeTimer.Stop();
    }

    public void Catch(Vector2 Velocity)
    {
        State = FishState.Fished;
        GravityScale = 0;
        this.Velocity = Velocity;
        foreach (CollisionShape2D hurtBox in GetChildren().Where(child => child.GetGroups().Contains("Hurtboxes")))
        {
            hurtBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        }
    }

    public virtual void Trigger()
	{
		Kill();
	}

	public virtual void Kill()
	{
		if (State == FishState.Dead) return;
		State = FishState.Dead;
        Velocity = Vector2.Zero;
        GravityScale = 0.6f;
		Sprite.Animation = "dead";
    }
}


