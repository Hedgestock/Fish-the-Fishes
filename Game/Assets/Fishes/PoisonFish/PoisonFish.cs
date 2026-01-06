using Godot;
using System;
using WaffleStock;

public partial class PoisonFish : Fish
{
    public override bool GetCaughtBy(IFisher by)
    {
        if (base.GetCaughtBy(by)) return false;

        EmitPoison();

        return true;
    }

    private void EmitPoison()
    {
        if (!IsActionable) return;

        GD.Print("POISON");
    }
}
