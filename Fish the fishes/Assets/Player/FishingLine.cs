using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using static Hook.Action;


public partial class FishingLine : CharacterBody2D, IFisher
{

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

    [Export]
    public Godot.Collections.Dictionary<string, PackedScene> Hooks;

    private Hook Hook;

    public List<IFishable> FishedThings { get; } = new List<IFishable>();

    private Vector2 BasePosition;
    private Vector2 Destination;
    private Vector2 Start;

    private bool _invincible;
    public bool IsInvincible { get { return _invincible; } }

    private AnimatedSprite2D Line;

    public override void _Ready()
    {
        EquipStuff();

        Line = GetNode<AnimatedSprite2D>("Line");
        Line.Play();

        GetTree().Root.SizeChanged += OnScreenResize;
    }

    public void EquipStuff()
    {
        // If this is the first launch of the game or if a now unavailable hook is equipped, this will be null
        string hookKey = UserData.Equipments.Where(e => e.Value.Type == EquipmentPiece.Type.Hook).FirstOrDefault(e => e.Value.IsEquipped).Key;

        if (hookKey == null) {
            // In the case of an invalid equippped item we default to standard
            hookKey = "StandardHook";
            UserData.Equipments[hookKey] = new UserData.EquipmentStatus(EquipmentPiece.Type.Hook, true);
        }

        if (Hook != null) { Hook.QueueFree(); }
        Hook = Hooks[hookKey].Instantiate<Hook>();
        AddChild(Hook);
        Hook.FishBox.BodyEntered += OnFishBoxBodyEntered;
        Hook.HitBox.BodyEntered += OnHitBoxBodyEntered;
        BasePosition = BasePosition = new Vector2(GameManager.ScreenSize.X / 2, 100 - Hook.AimOffset);
        Position = BasePosition;
        _invincible = false;

        ComputeScore();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Hook.State == Stopped) return;

        MoveAndSlide();

        if (Start.DistanceTo(Position) >= Start.DistanceTo(Destination))
        {
            switch (Hook.State)
            {
                case MovingDown:
                    Hook.State = MovingUp;
                    MoveTowardsCustom(new Vector2(BasePosition.X, -150 - Hook.AimOffset));
                    Line.Animation = "weighted";
                    break;
                case MovingUp:
                    // This avoids loosing on target mode when we fish nothing
                    if (FishedThings.Count > 0)
                        EmitSignal(SignalName.Score, ComputeScore());
                    Line.Animation = "loose";
                    goto case GettingHit;
                case GettingHit:
                    Hook.State = Resetting;
                    MoveTowardsCustom(BasePosition);
                    break;
                case Resetting:
                    Hook.Reset();
                    Hook.State = Stopped;
                    Velocity = new Vector2(0, 0);
                    break;
                case Stopped:
                default:
                    return;
            }
        }
    }


    private void MakeInvincible(Area2D area)
    {
        if (area == Hook.HitBox)
        {
            _invincible = true;
        }
    }

    private void MakeVincible(Area2D area)
    {
        if (area == Hook.HitBox)
        {
            _invincible = false;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Visible == false) return;
        // Mouse in viewport coordinates.
        if (@event is InputEventMouseButton eventMouseButton && @event.IsActionPressed("screen_tap") && Hook.CanMove(Hook.State))
        {
            Hook.State = MovingDown;
            MoveTowardsCustom(new Vector2(eventMouseButton.Position.X, eventMouseButton.Position.Y - Hook.AimOffset));
        }
    }

    public void MoveTowardsCustom(Vector2 position)
    {
        Destination = position;
        Start = Position;

        float computedSpeed = Speed;

        switch (Hook.State)
        {
            case MovingDown:
                computedSpeed = Speed * Hook.SpeedMultiplierDown + Hook.FlatSpeedModifierDown;
                break;
            case MovingUp:
                computedSpeed = Speed * Hook.SpeedMultiplierUp + Hook.FlatSpeedModifierUp;
                break;
            case Stopped:
                computedSpeed = 0;
                break;
            case GettingHit:
            case Resetting:
            default:
                break;
        }

        computedSpeed = Math.Max(computedSpeed, Speed / 4);

        Velocity = (position - Position).Normalized() * computedSpeed;
    }

    void OnFishBoxBodyEntered(Node2D body)
    {
        if (body is IFishable)
        {
            (body as IFishable).GetCaughtBy(this);
        }
    }

    void OnHitBoxBodyEntered(Node2D body)
    {
        if (body is Trash)
        {
            if (FishedThings.Count == 0 || IsInvincible) return;

            UserData.TrashCompendium[body.GetType().Name].Hit++;

            GetHit(DamageType.Trash);
        }
    }

    public void GetHit(DamageType damageType = DamageType.Trash)
    {
        if (FishedThings.Count == 0 || _invincible) return;
        EmitSignal(SignalName.Hit, (int)damageType);
        GetNode<AudioStreamPlayer2D>("HitSound").Play();
        AchievementsManager.OnHit(damageType);

        if (damageType == DamageType.Trash)
        {
            UserData.IncrementStatistic(Constants.TotalTrashesHit);
        }

        foreach (IFishable thing in FishedThings)
        {
            (thing as Node).CallDeferred(Node.MethodName.Reparent, GetParent());
            thing.IsCaught = false;
            if (GameManager.Mode != Game.Mode.GoGreen && thing is Fish)
            {
                (thing as Fish).Kill();
                UserData.IncrementStatistic(Constants.TotalLostFishes);
            }
        }

        Velocity = new Vector2(0, 0);
        Line.Animation = "hit";

        GetTree().CreateTimer(1).Timeout += () => { MoveTowardsCustom(Destination); Line.Animation = "loose"; FishedThings.Clear(); };
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

        UserData.SetHighStat(Constants.MaxFishedFishes, (uint)FishedThings.Count);
        UserData.IncrementStatistic(Constants.TotalFishedFishes, (uint)FishedThings.Count);

        foreach (Fish fish in FishedThings)
        {
            UpdateFishCompendium(fish);
            score *= fish.Multiplier;
            fish.QueueFree();
        }

        UserData.SetHighStat(Constants.MaxPointScored, (uint)score);
        UserData.IncrementStatistic(Constants.TotalPointsScored, (uint)score);

        return (int)score;

    }

    private int GoGreenScore()
    {
        int score = FishedThings.Where(thing => thing is Trash).Count();

        if (FishedThings.OfType<Fish>().Any())
        {
            UserData.IncrementStatistic(Constants.TotalEatenFishes, (uint)FishedThings.Where(thing => thing is Fish).Count());
            CallDeferred(MethodName.EmitSignal, SignalName.Hit, (int)DamageType.Default);
        }

        UserData.IncrementStatistic(Constants.TotalTrashesCleaned, (uint)FishedThings.Where(thing => thing is Trash).Count());

        foreach (Node thing in FishedThings)
        {
            if (thing is Trash) UserData.TrashCompendium[thing.GetType().Name].Cleaned++;

            thing.QueueFree();
        }

        return score;
    }

    private int TargetScore()
    {

        int score = FishedThings.Any(thing => thing.GetType().Name == GameManager.Target) ? 1 : 0;

        UserData.SetHighStat(Constants.MaxFishedFishes, (uint)FishedThings.Count);
        UserData.IncrementStatistic(Constants.TotalFishedFishes, (uint)FishedThings.Count);

        UserData.SetHighStat(Constants.MaxPointScored, (uint)score);
        UserData.IncrementStatistic(Constants.TotalPointsScored, (uint)score);

        foreach (Fish fish in FishedThings)
        {
            UpdateFishCompendium(fish);
            fish.QueueFree();
        }
        FishedThings.Clear();

        return score;
    }

    private int ScoringFunction(int num, int b = 3)
    {
        if (num <= 0) return 0;
        return (int)(num * MathF.Log(num, b) + 1);
    }

    private void UpdateFishCompendium(Fish fish)
    {
        string fishTypeName = fish.GetType().Name;
        UserData.FishCompendium[fishTypeName].Caught++;
        if (UserData.FishCompendium[fishTypeName].MaxSize < 0)
        {
            UserData.FishCompendium[fishTypeName].MaxSize = fish.ActualSize;
            UserData.FishCompendium[fishTypeName].MinSize = fish.ActualSize;
        }
        else if (UserData.FishCompendium[fishTypeName].MaxSize < fish.ActualSize)
        {
            UserData.FishCompendium[fishTypeName].MaxSize = fish.ActualSize;
        }
        else if (UserData.FishCompendium[fishTypeName].MinSize > fish.ActualSize)
        {
            UserData.FishCompendium[fishTypeName].MinSize = fish.ActualSize;
        }
        AchievementsManager.OnFishFished();
    }

    private void OnScreenResize()
    {
        if (Hook.State == Stopped)
        {
            BasePosition = new Vector2(GameManager.ScreenSize.X / 2, 100 - Hook.AimOffset);
            Position = BasePosition;
        }
    }
}
