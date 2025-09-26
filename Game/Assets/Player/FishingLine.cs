using Godot;
using WaffleStock;
using System;
using System.Collections.Generic;
using System.Linq;
using static Hook.Action;
using System.Reflection;

public partial class FishingLine : CharacterBody2D, IFisher
{
    public enum DamageType
    {
        Default,
        Trash,
        Electric
    }

    [Signal]
    public delegate void ScoreEventHandler(int score);

    [Signal]
    public delegate void HitEventHandler(int damageType);

    [Export]
    private AudioStreamPlayer2D ReelingSound;

    [Export]
    private uint Speed;

    private Hook Hook;
    private EquipmentPiece Line;
    private EquipmentPiece Weight;
    private EquipmentPiece Attractor;

    public List<IFishable> FishedThings { get; } = new List<IFishable>();

    private Vector2 BasePosition;
    private Vector2 Destination;
    private Vector2 Start;

    private bool _invincible;
    public bool IsInvincible { get { return _invincible; } }

    public override void _Ready()
    {
        EquipStuff();

        Line.Play();

        GetTree().Root.Connect(Viewport.SignalName.SizeChanged, Callable.From(OnScreenResize));
    }

    public void EquipStuff()
    {
        EquipHook();
        EquipLine();
        EquipWeigth();
        EquipAttractor();

        Hook.FishBox.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node2D>(OnFishBoxBodyEntered));
        Hook.HitBox.Connect(Area2D.SignalName.BodyEntered, Callable.From<Node2D>(OnHitBoxBodyEntered));
        BasePosition = BasePosition = new Vector2(GameManager.ScreenSize.X / 2, 100 - Hook.AimOffset);
        Position = BasePosition;
        _invincible = false;

        ComputeScore();
    }

    public void EquipHook()
    {
        EquipSingle(ref Hook, EquipmentPiece.Type.Hook, "StandardHook");
    }

    public void EquipLine()
    {
        EquipSingle(ref Line, EquipmentPiece.Type.Line, "StandardLine");
    }

    public void EquipWeigth()
    {
        EquipSingle(ref Weight, EquipmentPiece.Type.Weight, null);
    }

    public void EquipAttractor()
    {
        EquipSingle(ref Attractor, EquipmentPiece.Type.Attractor, null);
    }

    private void EquipSingle<PieceType>(ref PieceType piece, EquipmentPiece.Type pieceType, StringName fallbackPiece) where PieceType : EquipmentPiece
    {
        if (piece != null) RemoveChild(piece);

        // If this is the first launch of the game or if a now unavailable equipment piece is equipped, we use the fallback
        string equipmentKey = UserData.Equipments.Where(e => e.Value.Type == pieceType).FirstOrDefault(e => e.Value.IsEquipped).Key ?? fallbackPiece;

        if (equipmentKey != null)
        {
            // This is redudancy to equip a fallback
            UserData.Equipments[equipmentKey] = new UserData.EquipmentStatus(pieceType, true);
            piece = GD.Load<PackedScene>(Constants.EquipmentList[pieceType][equipmentKey]).Instantiate<PieceType>();
            AddChild(piece);
        }

        ComputeSpeed();
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
                    ReelingSound.Play();
                    break;
                case MovingUp:
                    // This avoids loosing on target mode when we fish nothing
                    if (FishedThings.Count > 0)
                        EmitSignal(SignalName.Score, ComputeScore());
                    Line.Animation = "loose";
                    ReelingSound.Stop();
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

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Visible == false) return;
        // Mouse in viewport coordinates.
        if (@event is InputEventMouseButton eventMouseButton && @event.IsActionPressed("screen_tap") && Hook.CanMove(Hook.State))
        {
            Hook.State = MovingDown;
            MoveTowardsCustom(new Vector2(eventMouseButton.Position.X, eventMouseButton.Position.Y - Hook.AimOffset));
        }
    }

    private float ComputedSpeedDown;
    private float ComputedSpeedUp;

    private EquipmentPiece DefaultPiece = new();

    private void ComputeSpeed()
    {
        ComputedSpeedUp = Speed * (Line ?? DefaultPiece).SpeedMultiplierUp * (Hook ?? DefaultPiece).SpeedMultiplierUp * (Weight ?? DefaultPiece).SpeedMultiplierUp * (Attractor ?? DefaultPiece).SpeedMultiplierUp
                                + (Line ?? DefaultPiece).FlatSpeedModifierUp + (Hook ?? DefaultPiece).FlatSpeedModifierUp + (Weight ?? DefaultPiece).FlatSpeedModifierUp + (Attractor ?? DefaultPiece).FlatSpeedModifierUp;
        ComputedSpeedDown = Speed * (Line ?? DefaultPiece).SpeedMultiplierDown * (Hook ?? DefaultPiece).SpeedMultiplierDown * (Weight ?? DefaultPiece).SpeedMultiplierDown * (Attractor ?? DefaultPiece).SpeedMultiplierDown
                                + (Line ?? DefaultPiece).FlatSpeedModifierDown + (Hook ?? DefaultPiece).FlatSpeedModifierDown + (Weight ?? DefaultPiece).FlatSpeedModifierDown + (Attractor ?? DefaultPiece).FlatSpeedModifierDown;
    }

    public void MoveTowardsCustom(Vector2 position)
    {
        Destination = position;
        Start = Position;

        float realSpeed = Speed;

        switch (Hook.State)
        {
            case MovingDown:
                realSpeed = ComputedSpeedDown;
                break;
            case MovingUp:
                realSpeed = ComputedSpeedUp;
                break;
            case Stopped:
                realSpeed = 0;
                break;
            case GettingHit:
            case Resetting:
            default:
                break;
        }

        realSpeed = Math.Clamp(realSpeed, Speed / 4, Speed * 2);

        Velocity = (position - Position).Normalized() * realSpeed;
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
        Hook.State = GettingHit;
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
                    break;
                case Game.Mode.GoGreen:
                    return Scoring.GoGreenScore();
                case Game.Mode.Target:
                    return Scoring.TargetScore(((IFisher)this).FlattenFishedThings(FishedThings));
                case Game.Mode.Training:
                    break;
                case Game.Mode.Zen:
                    break;
                case Game.Mode.Classic:
                case Game.Mode.TimeAttack:
                default:
                    return Scoring.ClassicScore(((IFisher)this).FlattenFishedThings(FishedThings));
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

    private void OnScreenResize()
    {
        if (Hook.State == Stopped)
        {
            BasePosition = new Vector2(GameManager.ScreenSize.X / 2, 100 - Hook.AimOffset);
            Position = BasePosition;
        }
    }
}
