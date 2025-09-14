using Godot;
using System;
using WaffleStock;

public partial class WaterEffect : TextureRect
{
    public override void _Ready()
    {
        base._Ready();
        Visible = UserSettings.WaterEffect;
    }
}
