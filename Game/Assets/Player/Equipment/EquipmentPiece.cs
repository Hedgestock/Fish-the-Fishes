using Godot;
using System;

public partial class EquipmentPiece : AnimatedSprite2D
{
    public enum Type
    {
        Hook,
        Line,
        Weight,
        Attractor,
    }

    [Export]
    public Type EquipmentType;
    [Export]
    public string EquipmentName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public string EquipmentDescription { get; set; }

    [ExportCategory("Stats")]
    [Export]
    public float SpeedMultiplierDown = 1;
    [Export]
    public float SpeedMultiplierUp = 1;
    [Export]
    public int FlatSpeedModifierDown = 0;
    [Export]
    public int FlatSpeedModifierUp = 0;
}
