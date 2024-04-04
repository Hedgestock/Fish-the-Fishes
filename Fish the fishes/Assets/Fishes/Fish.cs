using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Xml.Linq;


public partial class Fish : CharacterBody2D
{
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
    public bool IsAlive = true;
    public bool IsCaught = false;

    protected AnimatedSprite2D Sprite;

    public bool Actionable
    {
        get { return IsAlive && !IsCaught; }
    }

    public bool IsOnScreen
    {
        get { return GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").IsOnScreen(); }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (ActualSpeed == 0)
        {
            ActualSpeed = (float) GD.RandRange(MinSpeed, MaxSpeed) * (Flip ? -1 : 1);
		}

        Velocity = new Vector2(ActualSpeed, 0);

        if (Flip)
        {
            Scale *= new Vector2(-1, 1);
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

    public virtual void Catch(Vector2 Velocity)
    {
        IsCaught = true;
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
		if (!IsAlive) return;
        IsAlive = false;
        Velocity = Vector2.Zero;
        GravityScale = 0.6f;
		Sprite.Animation = "dead";
    }
}


