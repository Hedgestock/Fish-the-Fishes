using Godot;
using System;
using System.Linq;
using static Godot.WebSocketPeer;

public partial class Hook : EquipmentPiece
{
    public Area2D Area;

    [Export]
    private uint TotalMoves = 1;
    private int MovesLeft;
    public override void _Ready()
    {
        Area = GetNode<Area2D>("Area2D");
        Reset();
    }

    public void DisableHitbox(bool disabled)
    {
        foreach (CollisionShape2D hitbox in Area.GetChildren().Where(c => c is CollisionShape2D))
        {
            hitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, disabled);
        }
    }

    public bool CanMove(FishingLine.Action state)
    {
        return (state == FishingLine.Action.Stopped || state == FishingLine.Action.MovingDown) && MovesLeft-- > 0;
    }

    public void Reset() { MovesLeft = (int)TotalMoves; }
}
