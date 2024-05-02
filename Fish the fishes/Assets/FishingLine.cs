using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class FishingLine : CharacterBody2D, IFisher
{
    enum Action
    {
        Stopped,
        Moving,
        Fishing,
        Hit,
        Resetting
    }

    public enum DamageType
    {
        Default,
        Trash,
        Electric
    }

    [Signal]
    public delegate void ScoreEventHandler();

    [Signal]
    public delegate void HitEventHandler();

    [Export]
    private uint Speed;

    public List<IFishable> FishedThings { get; } = new List<IFishable>();

    private Vector2 Destination;
    private Vector2 Start;
    private Action State;
    private bool Invincible;

    private Vector2 BasePosition;
    private Area2D Area;
    private CollisionShape2D Hitbox;
    private AnimatedSprite2D Line;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BasePosition = new Vector2(GameManager.ScreenSize.X / 2, 50);
        Position = BasePosition;
        State = Action.Stopped;
        Invincible = false;
        Area = GetNode<Area2D>("Area2D");
        Hitbox = Area.GetNode<CollisionShape2D>("CollisionShape2D");
        Hitbox.Disabled = true;
        Line = GetNode<AnimatedSprite2D>("Line");
        Line.Play();

        GetTree().Root.SizeChanged += OnScreenResize;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

        if (State == Action.Stopped) return;

        MoveAndSlide();


        if (Start.DistanceTo(Position) > Start.DistanceTo(Destination))
        {
            switch (State)
            {
                case Action.Moving:
                    State = Action.Fishing;
                    MoveTowards(new Vector2(GameManager.ScreenSize.X / 2, -150));
                    Hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
                    Line.Animation = "weighted";
                    break;
                case Action.Fishing:
                    Hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
                    // This avoids loosing on target mode when we fish nothing
                    if (FishedThings.Count > 0)
                        EmitSignal(SignalName.Score, ComputeScore());
                    Line.Animation = "loose";
                    goto case Action.Hit;
                case Action.Hit:
                    State = Action.Resetting;
                    MoveTowards(BasePosition);
                    break;
                case Action.Resetting:
                    State = Action.Stopped;
                    Velocity = new Vector2(0, 0);
                    break;
                case Action.Stopped:
                default:
                    return;
            }
        }
    }


    private void setInvicibility(bool invincibility)
    {
        Invincible = invincibility;
    }

    private void MakeInvincible(Area2D area)
    {
        if (area == Area)
        {
            setInvicibility(true);
        }
    }

    private void MakeVincible(Area2D area)
    {
        if (area == Area)
        {
            setInvicibility(false);
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (State != Action.Stopped || Visible == false) return;
        // Mouse in viewport coordinates.
        if (@event is InputEventMouseButton eventMouseButton && @event.IsActionPressed("screen_tap"))
        {
            State = Action.Moving;
            MoveTowards(eventMouseButton.Position);
        }
    }

    private void MoveTowards(Vector2 position)
    {
        Destination = position;
        Start = Position;
        Velocity = (position - Position).Normalized() * Speed;
    }

    void _on_area_2d_body_entered(Node2D body)
    {
        if (body is IFishable)
        {
            (body as IFishable).GetCaughtBy(this);
        }
    }

    public void GetHit(DamageType damageType = DamageType.Trash)
    {
        if (FishedThings.Count == 0 || Invincible) return;
        EmitSignal(SignalName.Hit, (int)damageType);
        Hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        GetNode<AudioStreamPlayer2D>("HitSound").Play();

        if (damageType == DamageType.Trash)
        {
            UserData.Statistics[Constants.TotalTrashesHit] = UserData.Statistics.GetValueOrDefault(Constants.TotalTrashesHit) + 1;
        }

        foreach (IFishable thing in FishedThings)
        {
            (thing as Node).CallDeferred(Node.MethodName.Reparent, GetParent());
            thing.IsCaught = false;
            if (GameManager.Mode != Game.Mode.GoGreen && thing is Fish)
            {
                (thing as Fish).Kill();
                UserData.Statistics[Constants.TotalLostFishes] = UserData.Statistics.GetValueOrDefault(Constants.TotalLostFishes) + 1;
            }
        }

        Velocity = new Vector2(0, 0);
        Line.Animation = "hit";

        GetTree().CreateTimer(1).Timeout += () => { MoveTowards(Destination); Line.Animation = "loose"; FishedThings.Clear(); };
    }

    private int ComputeScore()
    {
        try
        {
            switch (GameManager.Mode)
            {

                case Game.Mode.Menu:
                    foreach (Node node in FishedThings)
                    {
                        node.QueueFree();
                    }
                    FishedThings.Clear();
                    return 0;
                case Game.Mode.GoGreen:
                    return GoGreenScore();
                case Game.Mode.Target:
                    return TargetScore();
                case Game.Mode.Training:
                    break;
                case Game.Mode.Zen:
                    break;
                case Game.Mode.Classic:
                case Game.Mode.TimeAttack:
                default:
                    return ClassicScore();
            }
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }
        finally
        {
            FishedThings.Clear();
        }
        return 0;
    }

    private int ClassicScore()
    {
        float score = 0;

        foreach (Fish fish in FishedThings)
        {
            score += fish.Value;
            UserData.FishCompendium[fish.GetType().Name].Caught++;
        }
        score = ScoringFunction((int)Math.Ceiling(score));
        foreach (Fish fish in FishedThings)
        {
            if (fish.IsNegative)
            {
                score = -score;
                break;
            }
        }

        UserData.Statistics[Constants.MaxFishedFishes] = (uint)Math.Max(UserData.Statistics.GetValueOrDefault(Constants.MaxFishedFishes), FishedThings.Count);
        UserData.Statistics[Constants.TotalFishedFishes] = UserData.Statistics.GetValueOrDefault(Constants.TotalFishedFishes) + (uint)FishedThings.Count;

        foreach (Fish fish in FishedThings)
        {
            score *= fish.Multiplier;
            fish.QueueFree();
        }

        UserData.Statistics[Constants.MaxPointScored] = (uint)Math.Max(UserData.Statistics.GetValueOrDefault(Constants.MaxPointScored), (int)score);
        UserData.Statistics[Constants.TotalPointsScored] = (uint)Math.Max(0, UserData.Statistics.GetValueOrDefault(Constants.TotalPointsScored) + (int)score);

        return (int)score;

    }

    private int GoGreenScore()
    {
        int score = FishedThings.Where(thing => thing is Trash).Count();

        if (FishedThings.OfType<Fish>().Any())
        {
            UserData.Statistics[Constants.TotalEatenFishes] = UserData.Statistics.GetValueOrDefault(Constants.TotalEatenFishes) + (uint)FishedThings.Where(thing => thing is Fish).Count();
            CallDeferred(MethodName.EmitSignal, SignalName.Hit, (int)DamageType.Default);
        }

        UserData.Statistics[Constants.TotalTrashesCleaned] = UserData.Statistics.GetValueOrDefault(Constants.TotalTrashesCleaned) + (uint)FishedThings.Where(thing => thing is Trash).Count();

        foreach (Node thing in FishedThings)
        {
            thing.QueueFree();
        }

        return score;
    }

    private int TargetScore()
    {
        int score = FishedThings.Any(thing => thing.GetType().Name == GameManager.Target) ? 1 : 0;

        UserData.Statistics[Constants.MaxFishedFishes] = (uint)Math.Max(UserData.Statistics.GetValueOrDefault(Constants.MaxFishedFishes), FishedThings.Count);
        UserData.Statistics[Constants.TotalFishedFishes] = UserData.Statistics.GetValueOrDefault(Constants.TotalFishedFishes) + (uint)FishedThings.Count;

        FishedThings.ForEach(thing =>
        {
            if (thing is Fish) UserData.FishCompendium[thing.GetType().Name].Caught++;
            (thing as Node).QueueFree();
        });
        FishedThings.Clear();

        UserData.Statistics[Constants.MaxPointScored] = (uint)Math.Max(UserData.Statistics.GetValueOrDefault(Constants.MaxPointScored), score);
        UserData.Statistics[Constants.TotalPointsScored] = (uint)Math.Max(0, UserData.Statistics.GetValueOrDefault(Constants.TotalPointsScored) + score);

        return score;
    }

    private int ScoringFunction(int num, int b = 3)
    {
        if (num <= 0) return 0;
        return (int)(num * MathF.Log(num, b) + 1);
    }

    private void OnScreenResize()
    {
        BasePosition = new Vector2(GameManager.ScreenSize.X / 2, 50);
    }
}
