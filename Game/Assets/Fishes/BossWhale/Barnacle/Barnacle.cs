using Godot;
using System;
using WaffleStock;

public partial class Barnacle : Fish
{
    public override IFishable GetCaughtBy(IFisher by)
    {
        if (!IsAlive) return this;
        ((BossWhale)GetParent()).RemoveBarnacle();
        Kill();
        return this;
    }

    public override void Kill()
    {
        CallDeferred(MethodName.Reparent, GetParent().GetParent());
        Velocity = new(GD.RandRange(-100, 100), -300);
        base.Kill();
    }

    protected override void Despawn()
    {
        if (!IsAlive)
            base.Despawn();
    }
}
