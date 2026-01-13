using Godot;
using System;
using WaffleStock;

public partial class Barnacle : Fish
{
    public override bool IsActionable => false;

    public override void _Ready()
    {
        // We skip the base ready function because even though it's of fish class for convenience
        // it is obvously not an autonomous fish at all
        //base._Ready();

        IsCaught = false;
        CantGetCaught = false;

        SetSize();

        SetScale();
    }
    public override bool GetCaughtBy(IFisher by)
    {
        if (!IsAlive) return false;
        Kill();
        return false;
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
