using Godot;
using System;
using WaffleStock;

public partial class PoisonFish : Fish
{
    public override IFishable GetCaughtBy(IFisher by)
    {
        if (!IsCaught)
            EmitPoison();

        return base.GetCaughtBy(by);
    }

    private void EmitPoison()
    {
        GD.Print("POISON");
    }
}
