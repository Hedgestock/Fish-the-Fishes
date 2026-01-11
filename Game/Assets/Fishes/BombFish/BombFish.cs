using Godot;
using Godot.Collections;
using System;
using System.Linq;
using WaffleStock;

public partial class BombFish : Fish
{
    //[Export]
    //public Array<Constants.Fishes> ImmuneToExplosion = new Array<Constants.Fishes>();

    [Export]
    Area2D ExplosionArea;

    public override bool GetCaughtBy(IFisher by)
    {
        Kill();
        return false;
    }

    public override void Kill()
    {
        if (!CanGetCaught) return;
        CanGetCaught = false;
        base.Kill();
        foreach (var fish in ExplosionArea.GetOverlappingBodies().OfType<Fish>())
        {
            fish.Kill();
        }
        QueueFree();
    }
}