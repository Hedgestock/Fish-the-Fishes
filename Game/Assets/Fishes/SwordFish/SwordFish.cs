using Godot;
using Godot.Collections;
using WaffleStock;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SwordFish : Fish, IFisher
{
    private enum Action
    {
        Swimming,
        Seeking,
        Launched,
        Leaving
    }

    [Export]
    public Array<Constants.Fishes> ImmuneToTargeting = new Array<Constants.Fishes>();

    [Export]
    public Array<Constants.Fishes> ImmuneToSkew = new Array<Constants.Fishes>();

    [ExportGroup("Behaviour")]
    [Export]
    private int MaxStrikes = 5;
    [Export]
    private int MinStrikes = 1;

    [ExportGroup("Attributes")]
    [Export]
    private CollisionShape2D HitBox;
    //[Export]
    //private GpuParticles2D Bubbles;

    public List<IFishable> FishedThings { get; } = new List<IFishable>();


    private int Strikes = 3;
    private Fish Target = null;
    private Action State = Action.Swimming;
    private float LaunchedSpeed;
    private Tween RotationTween;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        Strikes = GD.RandRange(MinStrikes, MaxStrikes);
        LaunchedSpeed = ActualSpeed;
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (State == Action.Launched)
        {
            TrackTarget();
        }
    }

    public override IFishable GetCaughtBy(IFisher by)
    {
        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        CleanCurrentBehaviors();
        return base.GetCaughtBy(by);
    }

    public override void Kill()
    {
        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        CleanCurrentBehaviors();
        base.Kill();
    }

    private void CleanCurrentBehaviors()
    {
        State = Action.Swimming;
        if (RotationTween != null) RotationTween.Kill();
        //Bubbles.Emitting = false;
    }

    private void SeekTarget()
    {
        if (!IsActionable || State == Action.Seeking) return;

        CleanCurrentBehaviors();

        if (Strikes <= 0)
        {
            Leave();
            return;
        }

        Sprite.Animation = "seek";

        Node[] fishes = GetTree().GetNodesInGroup("Fishes")
            .Where(fish => (fish as Fish).IsAlive && (fish as Fish).IsOnScreen && !FishedThings.Contains(fish as Fish) && !CheckImmunity(ImmuneToTargeting, fish.GetType()))
            .ToArray();

        if (fishes.Length == 0)
        {
            Leave();
            return;
        }

        Target = (Fish)fishes[(int)(GD.Randi() % fishes.Length)];

        Velocity = Vector2.Zero;

        State = Action.Seeking;

        RotationTween = RotateAtConstantSpeed(GetDirectionTo(Target).Angle());
        RotationTween.TweenCallback(Callable.From(() => CallDeferred("Launch")));
    }

    private void Launch()
    {
        if (!IsActionable) return;

        if (!IsInstanceValid(Target)|| FishedThings.Contains(Target))
        {
            State = Action.Swimming;
            SeekTarget();
            return;
        }

        Strikes--;

        LaunchedSpeed = TrackTarget(true);

        Sprite.Animation = "dash";
        //Bubbles.Emitting = true;
        //Bubbles.AmountRatio = Mathf.Min(LaunchedSpeed/2000, 1);

        State = Action.Launched;
    }

    private void Leave()
    {
        Sprite.Animation = IsAlive ? "alive" : "dead";
        if (!IsActionable) return;
        Velocity = new Vector2(ActualSpeed * (Flip ? -1 : 1), 0);

        RotationTween = RotateAtConstantSpeed(Velocity.Angle());

        State = Action.Leaving;
    }

    private void OnFishSkewered(Node2D body)
    {
        if (!(body is Fish) || FishedThings.Contains(body as Fish) || body == this || !IsActionable || CheckImmunity(ImmuneToSkew, body.GetType())) return;

        Fish Skew = body as Fish;

        Skew = Skew.GetCaughtBy(this) as Fish;
        Skew.Kill();


        if (FishedThings.Contains(Target))
        {
            Velocity = Vector2.Zero;
            SeekTarget();
        }
    }

    #region helpers
    private Vector2 GetDirectionTo(Fish target)
    {
        return target.GlobalPosition - GlobalPosition;
    }

    private float TrackTarget(bool atLaunch = false)
    {

        if (!IsActionable)
        {
            GD.PrintErr($"{Name}({GetType()}) trying to track a target when it's not actionable");
            return 0;
        }

        if (!IsInstanceValid(Target) || FishedThings.Contains(Target))
        {
            GD.PrintErr($"{Name}({GetType()}) trying to track invalid or contained target");
            GD.PrintErr($"{!IsInstanceValid(Target)} {FishedThings.Contains(Target)}");

            State = Action.Swimming;
            SeekTarget();
            return 0;
        }

        Velocity = GetDirectionTo(Target);

        if (atLaunch)
        {
            if (Velocity.Length() < ActualSpeed)
                Velocity = Velocity.Normalized() * ActualSpeed;
        }
        else
        {
            Velocity = Velocity.Normalized() * LaunchedSpeed;
        }

        Rotation = Velocity.Angle();

        return Velocity.Length();
    }

    private Tween RotateAtConstantSpeed(float angle)
    {
        Tween tween = CreateTween();
        angle = Mathf.LerpAngle(Rotation, angle, 1);
        // This timig allows for constant rotation velocity (1s for 180 degrees)
        float duration = Mathf.Abs(Rotation - angle) / Mathf.Pi;
        tween.TweenProperty(this, "rotation", angle, duration);

        return tween;
    }
    #endregion helpers
}
