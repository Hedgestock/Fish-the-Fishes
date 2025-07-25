using Godot;
using Godot.Collections;
using WaffleStock;
using System;
using System.Linq;
using System.Diagnostics;



public partial class Fish : CharacterBody2D, IFishable, IDescriptible
{
    [Signal]
    public delegate void GotFishedEventHandler(Node by);

    [Export]
    protected AnimatedSprite2D Sprite;
    [Export]
    protected AudioStreamPlayer2D Sound;
    [Export]
    protected VisibleOnScreenNotifier2D VisibleOnScreenNotifier;

    [ExportGroup("Compendium")]
    [Export]
    public string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public string CompendiumDescription { get; set; }

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
    public float MaxSpeed = 250;
    [Export(PropertyHint.Range, "0,1")]
    private float TajectoryDeviation = 0.1f;
    [Export]
    protected Vector2 SpawnRange = new(0, 1);
    [Export]
    public float AverageSize = 100;
    [Export]
    private float SizeDeviation = 0.1f;

    protected float GravityScale = 0;

    public bool Flip = false;
    public float ActualSpeed = 0;
    public Vector2 TravelAxis = Vector2.Zero;
    protected float ActualSizeVariation = 0;
    public bool IsAlive = true;
    public bool IsCaught { get; set; }
    public bool IsInDisplay { get; set; }

    public float ActualSize
    {
        get { return AverageSize * ActualSizeVariation; }
    }

    public bool IsHuge
    {
        get { return ActualSize > 75; }
    }

    public bool IsActionable
    {
        get { return IsAlive && !IsCaught && !IsInDisplay; }
    }

    public bool IsOnScreen
    {
        get { return VisibleOnScreenNotifier.IsOnScreen(); }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Sprite.Animation = "alive";
        Sprite.Play();

        if (IsInDisplay) return;
        NotifySpawn();

        if (GameManager.Mode != Game.Mode.Menu)
        {
            Sound.Play();
        }

        IsCaught = false;

        // If the inheriting class did not set the spawning Posision, we do it now.
        if (Position == Vector2.Zero)
        {
            if (GameManager.Flip != 0)
                Flip = GameManager.Flip > 0;
            else
                Flip = (GD.Randi() % 2) != 0;
            float positionOffset = VisibleOnScreenNotifier.Rect.Size.X * VisibleOnScreenNotifier.Scale.X / 2;
            Position = new Vector2(
                Flip ? GameManager.ScreenSize.X + positionOffset : -positionOffset,
                (float)GD.RandRange(GameManager.ScreenSize.Y * SpawnRange.X, GameManager.ScreenSize.Y * SpawnRange.Y)
            );
        }

        // If the inheriting class did not set the ActualSpeed, we do it now.
        if (ActualSpeed == 0)
        {
            ActualSpeed = (float)GD.RandRange(MinSpeed, MaxSpeed);
        }

        // If the inheriting class did not set the TravelAxis, we do it now.
        if (TravelAxis == Vector2.Zero)
        {
            // Here, we invert the Flip condition to get a point that's on the opposite side of the spawning Position
            float trajectoryVariation = GameManager.ScreenSize.Y * TajectoryDeviation;
            Vector2 objective = new Vector2(
                !Flip ? GameManager.ScreenSize.X : 0,
                (float)GD.RandRange(
                    MathF.Max(0, Position.Y - trajectoryVariation),
                    MathF.Min(Position.Y + trajectoryVariation, GameManager.ScreenSize.Y)
                    )
                );
            TravelAxis = (objective - Position).Normalized();
        }

        if (Flip)
        {
            Scale = new Vector2(-1, 1);
        }

        Velocity = TravelAxis * ActualSpeed;
        Rotation = (float)(TravelAxis.Angle() - (Flip ? Mathf.Pi : 0));


        // If the inheriting class did not set the ActualSizeVariation, we do it now.
        if (ActualSizeVariation == 0)
        {
            ActualSizeVariation = (float)Mathf.Max(0.01, GD.Randfn(1, SizeDeviation));
        }

        Scale *= ActualSizeVariation;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsInDisplay) return;
        var velocity = Velocity;
        velocity.Y += (float)delta * GravityScale * (int)ProjectSettings.GetSetting("physics/2d/default_gravity");
        Velocity = velocity;
        MoveAndSlide();
    }

    public virtual IFishable GetCaughtBy(IFisher by)
    {
        //TODO: Fix the frame by frame call in the fishing line

        if (by == this)
        {
            GD.PrintErr(by, " is ", this);
            return this;
        }

        if (by.FishedThings.Contains(this) || (by as Node).IsAncestorOf(this))
            return this; // This avoids multiple calls on reparenting

        Velocity = Vector2.Zero;
        GravityScale = 0;
        if (IsAlive) Sprite.Animation = "alive";

        var parent = GetParent();
        Callable.From(() =>
            {
                Reparent(by as Node);
                if (parent is IFisher fisher)
                    fisher.FishedThings.Remove(this);
            }
            ).CallDeferred();

        by.FishedThings.Add(this);
        IsCaught = true;

        EmitSignal(SignalName.GotFished, by as Node);
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

    private void Despawn()
    {
        if (!IsCaught && !IsInDisplay)
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

    protected bool FishListContains(Array<Constants.Fishes> List, Type FishType)
    {
        return List.Select(i => i.ToString()).Contains(FishType.ToString());
    }
}


