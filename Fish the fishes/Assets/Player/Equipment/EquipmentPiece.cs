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
    [Export(PropertyHint.MultilineText)]
    public string EquipmentDescription { get; set; }

    [ExportCategory("Stats")]
    [Export]
    public float SpeedMultiplier = 1;
    [Export]
    public int FlatSpeedModifier = 0;
}
