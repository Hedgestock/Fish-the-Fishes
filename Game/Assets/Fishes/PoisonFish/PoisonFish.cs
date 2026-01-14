using Godot;
using System;
using WaffleStock;

public partial class PoisonFish : Fish
{
    public override void GetCaughtBy(IFisher fisher)
    {
        base.GetCaughtBy(fisher);

        EmitPoison();
    }

    private void EmitPoison()
    {
        if (!IsActionable) return;

        GD.Print("POISON");
    }
}
