using Godot;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Linq;
using System.Xml.Linq;


public partial class Fish : CharacterBody2D, IFishable
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
            ActualSpeed = (float)GD.RandRange(MinSpeed, MaxSpeed);
        }

        Velocity = new Vector2(ActualSpeed * (Flip ? -1 : 1), 0);

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

    public virtual void GetCaughtBy(IFisher by)
    {
        if (by.FishedThings.Contains(this)) return; // This avoids multiple calls on reparenting
        var parent = GetParent();
        if (IsCaught && parent is IFishable)
        {
            (parent as IFishable).GetCaughtBy(by);
            return;
        };
        if (this is IFisher)
        {
            by.FishedThings.AddRange((this as IFisher).FishedThings);
            (this as IFisher).FishedThings.Clear();
        }
        IsCaught = true;
        GravityScale = 0;
        Velocity = Vector2.Zero;
        by.FishedThings.Add(this);
        CallDeferred(Node.MethodName.Reparent, by as Node);
    }

    public virtual void Trigger()
    {
        Kill();
    }

    public virtual void Kill()
    {
        IsAlive = false;
        Sprite.Animation = "dead";
        if (!IsCaught) GravityScale = 0.6f;
    }
}


