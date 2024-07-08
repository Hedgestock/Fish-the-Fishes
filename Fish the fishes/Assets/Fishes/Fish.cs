using Godot;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Linq;



public partial class Fish : CharacterBody2D, IFishable, IDescriptible
{
    [Export]
    protected AnimatedSprite2D Sprite;
    [Export]
    protected AudioStreamPlayer2D Sound;

    [ExportGroup("Compendium")]
    [Export]
    public string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public string CompendiumDescription { get; set; }
    [Export]
    public float Size = 100;

    [ExportGroup("Scoring")]
    [Export]
    public float Value = 1;
    [Export]
    public uint Multiplier = 1;
    [Export]
    public bool IsNegative = false;

    [ExportGroup("Behaviour")]
    [Export]
    private float MinSpeed = 150;
    [Export]
    private float MaxSpeed = 250;
    [Export]
    private float SizeDeviation = 0.1f;

    private float GravityScale = 0;

    public bool Flip = false;
    public float ActualSpeed = 0;
    public float ActualSizeVariation = 0;
    public bool IsAlive = true;
    public bool IsCaught { get; set; }

    public bool IsHuge
    {
        get { return Size * ActualSizeVariation > 70; }
    }

    public bool IsActionable
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

        if (GameManager.Mode != Game.Mode.Menu)
        {
            Sound.Play();
        }

        IsCaught = false;

        // If the inherited class did not set the spawning Posision, we do it now
        if (Position == Vector2.Zero)
        {
            Flip = (GD.Randi() % 2) != 0;
            Position = new Vector2(Flip ? GameManager.ScreenSize.X + 200 : -200, (float)GD.RandRange(0, GameManager.ScreenSize.Y));
        }

        // If the inherited class did not set the ActualSpeed, we do it now
        if (ActualSpeed == 0)
        {
            ActualSpeed = (float)GD.RandRange(MinSpeed, MaxSpeed);
        }

        Velocity = new Vector2(ActualSpeed * (Flip ? -1 : 1), 0);

        if (Flip)
        {
            Scale = new Vector2(-1, 1);
        }


        // If the inherited class did not set the ActualSizeVariation, we do it now
        if (ActualSizeVariation == 0)
        {
            ActualSizeVariation = (float)GD.Randfn(1, SizeDeviation);
        }

        Scale *= ActualSizeVariation;

        Sprite.Animation = "alive";
        Sprite.Play();
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

        if (by == this)
        {
            GD.PrintErr(by, " is ", this);
            return this;
        }

        if (by.FishedThings.Contains(this) || (by as Node).GetChildren().Contains(this))
            return this; // This avoids multiple calls on reparenting

        Velocity = Vector2.Zero;
        GravityScale = 0;
        if (IsAlive) Sprite.Animation = "alive";

        var parent = GetParent();
        if (IsCaught)
        {
            // In case we are already caught by another fishable thing, we make that parent the target of the catching action
            if (parent is IFishable)
                return (parent as IFishable).GetCaughtBy(by);

            // In case we are already caught by a non fishable thing, we make sure that we are removed from its list ("stolen")
            // TO FIX: the stolen fish usually gets instantly recaught, leading to the thief to be caught as well
            (parent as IFisher).FishedThings.Remove(this);
        };

        by.FishedThings.Add(this);
        IsCaught = true;


        // In case we are a fisher thing, we make sure to give all of our fished things to what is currently catching us
        if (this is IFisher)
        {
            by.FishedThings.AddRange((this as IFisher).FishedThings);
            (this as IFisher).FishedThings.Clear();
        }

        CallDeferred(Node.MethodName.Reparent, by as Node);
        return this;
    }

    public virtual void Trigger()
    {
        Kill();
    }

    public virtual void Kill()
    {
        if (!IsCaught) GravityScale = 0.6f;
        if (!IsAlive) return;
        IsAlive = false;
        Sprite.Animation = "dead";
        Sound.Stop();
    }

    protected void Despawn()
    {
        if (!IsCaught)
        {
            QueueFree();
        }
    }

    protected void NotifySpawn()
    {
        if (GameManager.Mode == Game.Mode.Menu) return;
        if (UserData.FishCompendium.TryGetValue(GetType().Name, out UserData.FishCompendiumEntry entry)) entry.Seen++;
        else UserData.FishCompendium[GetType().Name] = new UserData.FishCompendiumEntry();
    }

    protected bool CheckImmunity(Array<Constants.Fishes> ImmunityList, Type FishType)
    {
        return ImmunityList.Select(i => i.ToString()).Contains(FishType.ToString());
    }
}


