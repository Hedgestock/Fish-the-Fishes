using Godot;
using Godot.Collections;
using System;
using WaffleStock;

public partial class BombFish : Fish
{
    [Export]
    public Array<Constants.Fishes> ImmuneToAttraction = new Array<Constants.Fishes>();

    [Export]
    Area2D ExplosionArea;

    public override bool GetCaughtBy(IFisher by)
    {
        return base.GetCaughtBy(by);
    }
}