using Godot;
using System;
using System.Linq;

public partial class Hook : EquipmentPiece
{
    public enum Action
    {
        Stopped,
        MovingDown,
        MovingUp,
        GettingHit,
        Resetting
    }

    public Area2D FishBox;
    public Area2D HitBox;

    [Export]
    public int AimOffset = 50;


    [Export]
    private uint TotalMoves = 1;
    private int MovesLeft;

    protected Action _state = Action.Stopped;
    public virtual Action State
    {
        get { return _state; }
        set
        {
            _state = value;
            DisableHitbox(_state != Action.MovingUp);
            DisableFishBox(_state != Action.MovingUp);
        }
    }

    public bool CanMove
    {
        get { return (State == Action.Stopped || State == Action.MovingDown) && MovesLeft-- > 0; }
    }

    public override void _Ready()
    {
        FishBox = GetNode<Area2D>("FishBox");
        HitBox = GetNode<Area2D>("HitBox");
        Reset();
    }

    protected void DisableHitbox(bool disabled)
    {
        foreach (CollisionShape2D hitbox in HitBox.GetChildren().Where(c => c is CollisionShape2D))
        {
            hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, disabled);
        }
    }

    protected void DisableFishBox(bool disabled)
    {
        foreach (CollisionShape2D fishbox in FishBox.GetChildren().Where(c => c is CollisionShape2D))
        {
            fishbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, disabled);
        }
    }

    public void Reset() { MovesLeft = (int)TotalMoves; }
}
