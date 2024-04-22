using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;


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
                    FishedThings.Clear();
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
        if (area == this.Area)
        {
            setInvicibility(true);
        }
    }

    private void MakeVincible(Area2D area)
    {
        if (area == this.Area)
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
        else if (FishedThings.Count > 0 && body is Trash && !Invincible)
        {
            EmitSignal(SignalName.Hit);
            Hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
            GetNode<AudioStreamPlayer>("HitSound").Play();

            UserData.Instance.Statistics[Constants.TotalTrashesHit] = UserData.Instance.Statistics.GetValueOrDefault(Constants.TotalTrashesHit) + 1;
            UserData.Instance.Statistics[Constants.TotalLostFishes] = UserData.Instance.Statistics.GetValueOrDefault(Constants.TotalLostFishes) + (uint)FishedThings.Count;

            foreach (Fish fish in FishedThings)
            {
                GD.Print("killing fish ", fish);
                fish.Kill();
                fish.CallDeferred(Node.MethodName.Reparent, GetParent());
            }
            Velocity = new Vector2(0, 0);
            Line.Animation = "hit";
            GetTree().CreateTimer(1).Timeout += () => { MoveTowards(Destination); Line.Animation = "loose"; FishedThings.Clear(); };
        }
    }

    private int ComputeScore()
    {
        float score = 0;
        try
        {
            foreach (Fish fish in FishedThings)
            {
                score += fish.Value;
                UserData.Instance.Compendium[fish.GetType().Name].Caught++;
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

            UserData.Instance.Statistics[Constants.MaxPointScored] = (uint) Math.Max(UserData.Instance.Statistics.GetValueOrDefault(Constants.MaxPointScored), score);
            UserData.Instance.Statistics[Constants.TotalPointsScored] = UserData.Instance.Statistics.GetValueOrDefault(Constants.TotalPointsScored) + (uint)score;

            return (int)score;
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
            return 0;
        }

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
