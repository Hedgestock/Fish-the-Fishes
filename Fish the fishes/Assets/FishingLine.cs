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
    private GameManager GM;
    private Area2D Area;
    private CollisionShape2D Hitbox;
    private AnimatedSprite2D Line;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GM = GetNode<GameManager>("/root/GameManager");

        BasePosition = new Vector2(GM.ScreenSize.X / 2, 50);
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
                    MoveTowards(new Vector2(GM.ScreenSize.X / 2, -150));
                    Hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
                    Line.Animation = "weighted";
                    break;
                case Action.Fishing:
                    Hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
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
        GetNode<AudioStreamPlayer>("HitSound").Play();

        if (damageType == DamageType.Trash)
        {
            UserData.Instance.Statistics[Constants.TotalTrashesHit] = UserData.Instance.Statistics.GetValueOrDefault(Constants.TotalTrashesHit) + 1;
        }

        foreach (IFishable thing in FishedThings)
        {
            (thing as Node).CallDeferred(Node.MethodName.Reparent, GetParent());
            thing.IsCaught = false;
            if (GM.Mode != Game.Mode.GoGreen && thing is Fish)
            {
                (thing as Fish).Kill();
                UserData.Instance.Statistics[Constants.TotalLostFishes] = UserData.Instance.Statistics.GetValueOrDefault(Constants.TotalLostFishes) + 1;
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
            switch (GM.Mode)
            {

                case Game.Mode.GoGreen:
                    return GoGreenScore();
                case Game.Mode.Target:
                    break;
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
            UserData.Instance.FishCompendium[fish.GetType().Name].Caught++;
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

        UserData.Instance.Statistics[Constants.MaxFishedFishes] = (uint)Math.Max(UserData.Instance.Statistics.GetValueOrDefault(Constants.MaxFishedFishes), FishedThings.Count);
        UserData.Instance.Statistics[Constants.TotalFishedFishes] = UserData.Instance.Statistics.GetValueOrDefault(Constants.TotalFishedFishes) + (uint)FishedThings.Count;

        foreach (Fish fish in FishedThings)
        {
            score *= fish.Multiplier;
            fish.QueueFree();
        }

        UserData.Instance.Statistics[Constants.MaxPointScored] = (uint)Math.Max(UserData.Instance.Statistics.GetValueOrDefault(Constants.MaxPointScored), (int)score);
        UserData.Instance.Statistics[Constants.TotalPointsScored] = (uint)Math.Max(0, UserData.Instance.Statistics.GetValueOrDefault(Constants.TotalPointsScored) + (int)score);

        return (int)score;

    }

    private int GoGreenScore()
    {
        int score = FishedThings.Where(thing => thing is Trash).Count();

        if (FishedThings.OfType<Fish>().Any())
        {
            UserData.Instance.Statistics[Constants.TotalEatenFishes] = UserData.Instance.Statistics.GetValueOrDefault(Constants.TotalEatenFishes) + (uint)FishedThings.Where(thing => thing is Fish).Count();
            CallDeferred(MethodName.EmitSignal, SignalName.Hit, (int)DamageType.Default);
        }

        UserData.Instance.Statistics[Constants.TotalTrashesCleaned] = UserData.Instance.Statistics.GetValueOrDefault(Constants.TotalTrashesCleaned) + (uint)FishedThings.Where(thing => thing is Trash).Count();

        foreach (Node thing in FishedThings)
        {
            thing.QueueFree();
        }

        return score;
    }
    private int ScoringFunction(int num, int b = 3)
    {
        if (num <= 0) return 0;
        return (int)(num * MathF.Log(num, b) + 1);
    }

    private void OnScreenResize()
    {
        BasePosition = new Vector2(GM.ScreenSize.X / 2, 50);
    }
}
