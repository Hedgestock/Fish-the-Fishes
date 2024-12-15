using Godot;
using System;
using static Godot.WebSocketPeer;

public partial class Hook : EquipmentPiece
{
    public Area2D Area;
    public CollisionShape2D Hitbox;

    [Export]
    private uint TotalMoves = 1;
    private int MovesLeft;
    public override void _Ready()
    {
        Area = GetNode<Area2D>("Area2D");
        Hitbox = Area.GetNode<CollisionShape2D>("CollisionShape2D");
        Reset();
    }

    public bool CanMove(FishingLine.Action state)
    {
        return (state == FishingLine.Action.Stopped || state == FishingLine.Action.Moving) && MovesLeft-- > 0;
    }

    public void Reset() { MovesLeft = (int)TotalMoves; }
}
