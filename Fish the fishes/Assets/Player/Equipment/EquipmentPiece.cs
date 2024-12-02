using Godot;
using System;

public partial class EquipmentPiece : AnimatedSprite2D
{
    public enum Type
    {
        Hook,
        Line,
        Bait,
        Lure,
        Weight,
        Cork
    }

    [Export]
    public string EquipmentName { get; set; }
    [Export]
    public string EquipmentDescription { get; set; }
    public bool Locked = true;
}
