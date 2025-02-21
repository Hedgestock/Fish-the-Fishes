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

    public Area2D Area;

    [Export]
    private uint TotalMoves = 1;
    private int MovesLeft;

    public Vector2 BasePosition;

    protected Action _state = Action.Stopped;
    public virtual Action State
    {
        get { return _state; }
        set
        {
            _state = value;
            DisableHitbox(_state != Action.MovingUp);
        }
    }

    public override void _Ready()
    {
        Area = GetNode<Area2D>("Area2D");
        BasePosition = new Vector2(GameManager.ScreenSize.X / 2, 50);
        Reset();

        GetTree().Root.SizeChanged += OnScreenResize;
    }

    protected void DisableHitbox(bool disabled)
    {
        foreach (CollisionShape2D hitbox in Area.GetChildren().Where(c => c is CollisionShape2D))
        {
            hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, disabled);
        }
    }

    public bool CanMove(Action state)
    {
        return (state == Action.Stopped || state == Action.MovingDown) && MovesLeft-- > 0;
    }

    public void Reset() { MovesLeft = (int)TotalMoves; }

    private void OnScreenResize()
    {
        BasePosition = new Vector2(GameManager.ScreenSize.X / 2, 50);
    }
}
