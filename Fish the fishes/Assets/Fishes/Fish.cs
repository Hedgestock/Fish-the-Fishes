using Godot;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Linq;
using System.Xml.Linq;


public abstract partial class Fish : CharacterBody2D, IFishable
{
    public static string CompendiumName = "Fish";
    public static string CompendiumDescription = "This is a fish";

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
        NotifySpawn();

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

    public virtual IFishable GetCaughtBy(IFisher by)
    {
        if (by == this || (by as Node).GetChildren().Contains(this))
        {
            GD.PrintErr(by, " is already a parent of ", this);
            throw new Exception();
        }

        if (by.FishedThings.Contains(this))
            return this; // This avoids multiple calls on reparenting

        Velocity = Vector2.Zero;
        GravityScale = 0;
        if (IsAlive) Sprite.Animation = "alive";

        // In case we are already caught by another fishable thing, we make that the target of the catching action
        var parent = GetParent();
        if (IsCaught && parent is IFishable)
        {
            return (parent as IFishable).GetCaughtBy(by);
        };
        // In case we are a fisher thing, we make sure to give all of our fished things to what is currently catching us
        if (this is IFisher)
        {
            by.FishedThings.AddRange((this as IFisher).FishedThings);
            (this as IFisher).FishedThings.Clear();
        }
        IsCaught = true;
        by.FishedThings.Add(this);
        CallDeferred(Node.MethodName.Reparent, by as Node);
        return this;
    }

    public virtual void Trigger()
    {
        Kill();
    }

    public virtual void Kill(bool whileCaught = false)
    {
        IsAlive = false;
        Sprite.Animation = "dead";
        if (!whileCaught) GravityScale = 0.6f;
    }

    protected void NotifySpawn()
    {
        if (UserData.Instance.Compendium.TryGetValue(GetType().Name, out UserData.CompendiumEntry entry))
        {
            entry.Seen++;
        }
        else
        {
            UserData.Instance.Compendium[GetType().Name] = new UserData.CompendiumEntry();
        }
    }
}


