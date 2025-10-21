using Godot;
using System;
using WaffleStock;

public partial class Barnacle : Fish
{
    override public IFishable GetCaughtBy(IFisher by)
    {
        if (!IsAlive) return this;
        Kill();
        CallDeferred(MethodName.Reparent, GetParent().GetParent());
        Velocity = new(GD.RandRange(-100,100), -300);
        return this;
    }

    protected override void Despawn()
    {
        if (!IsAlive)
            base.Despawn();
    }
}
