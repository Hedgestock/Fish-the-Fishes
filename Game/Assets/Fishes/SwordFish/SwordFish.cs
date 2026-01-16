using Godot;
using Godot.Collections;
using WaffleStock;
using System;
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
    [Export]
    private Bubbles Bubbles;

    private int Strikes = 3;
    private Fish Target = null;
    private Action State = Action.Swimming;
    private float LaunchedSpeed;
    private Tween RotationTween;

    private Callable TargetGotFished;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        if (IsInDisplay) return;
        Strikes = GD.RandRange(MinStrikes, MaxStrikes);
        LaunchedSpeed = ActualSpeed;
        ImmuneToTargeting.AddRange(ImmuneToSkew);

        //Bubbles.Reparent(FindParent("Game"));
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!IsActionable) return;
        if (State == Action.Launched)
        {
            TrackTarget();
        }
    }

    public override void GetCaughtBy(IFisher fisher)
    {
        base.GetCaughtBy(fisher);

        HitBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        CleanCurrentBehaviors();
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
        Bubbles.Emitting = false;
    }

    //public override void _Notification(int what)
    //{
    //    if (what == NotificationPredelete)
    //    {
    //        Bubbles.DelayedDespawn();
    //    }
    //    base._Notification(what);
    //}

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
            .Where(fish => (fish as Fish).IsAlive && (fish as Fish).IsOnScreen && !(this as IFisher).FlattennedFishedThings().Contains(fish as Fish) && !FishListContains(ImmuneToTargeting, fish.GetType()))
            .ToArray();

        if (fishes.Length == 0)
        {
            GD.Print($"{Name}({GetType()}) didn't find any suitable target");
            Leave();
            return;
        }

        Target = (Fish)fishes[(int)(GD.Randi() % fishes.Length)];

        TargetGotFished = Callable.From((Node by) =>
        {
            if (!this.IsAncestorOf(by)) return;
            ChangeTarget();
        });

        Target.Connect(Fish.SignalName.GotFished, TargetGotFished);

        GD.Print($"{Name}({GetType()}) is looking for target {Target.Name}");

        Velocity = Vector2.Zero;

        State = Action.Seeking;

        RotationTween = RotateAtConstantSpeed(GetDirectionTo(Target).Angle());
        RotationTween.TweenCallback(Callable.From(() => CallDeferred("Launch")));
    }

    private void Launch()
    {
        if (!IsActionable) return;

        if (!IsInstanceValid(Target) || (this as IFisher).FlattennedFishedThings().Contains(Target))
        {
            State = Action.Swimming;
            SeekTarget();
            return;
        }

        Strikes--;

        LaunchedSpeed = TrackTarget(true);

        Sprite.Animation = "dash";
        Bubbles.Emitting = true;
        Bubbles.AmountRatio = LaunchedSpeed / MaxSpeed;


        State = Action.Launched;
        GD.Print($"{Name}({GetType()}) is launching at target {Target.Name}");

    }

    private void Leave()
    {
        Sprite.Animation = IsAlive ? "alive" : "dead";
        if (!IsActionable) return;
        Velocity = TravelAxis * ActualSpeed;

        RotationTween = RotateAtConstantSpeed(Velocity.Angle());

        State = Action.Leaving;
    }

    private void OnFishSkewered(Node2D body)
    {
        if (!(body is IFishable Skew) || body.IsAncestorOf(this) || FishListContains(ImmuneToSkew, body.GetType()) || body == this || !IsActionable) return;

        if (Skew.EscapeOrGetCaughtBy(this))
        {
            if (Target == Skew) // Here we reached the target, but failed to skewer it
                ChangeTarget();
            return;
        }

        if (Skew is Fish fish)
            fish.Kill();

        if (Target == Skew || body.IsAncestorOf(Target))
            ChangeTarget();
    }

    private void ChangeTarget()
    {
        Velocity = Vector2.Zero;
        Target.Disconnect(Fish.SignalName.GotFished, TargetGotFished);
        State = Action.Swimming;
        SeekTarget();
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

        if (!IsInstanceValid(Target) || (this as IFisher).FlattennedFishedThings().Contains(Target))
        {
            GD.PrintErr($"{Name}({GetType()}) trying to track invalid or contained target");
            GD.PrintErr($"invalid: {!IsInstanceValid(Target)} contained: {(this as IFisher).FlattennedFishedThings().Contains(Target)}");

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

        //GD.Print($"{Name}({GetType()}) is tracking target {Target.Name}");

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
