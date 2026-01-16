using Godot;
using System;
using System.Linq;
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

    public override bool Escape(IFisher fisher)
    {
        Kill();
        return true;
    }

    public override void GetCaughtBy(IFisher fisher)
    {
        Kill();
    }

    public override void Kill()
    {
        if (!IsAlive) return;
        //CallDeferred(MethodName.Reparent, GetParent().GetParent());
        CallDeferred(MethodName.Reparent, GetTree().Root.GetChildren().OfType<Game>().First());

        Velocity = new(GD.RandRange(-100, 100), -300);
        base.Kill();

        EmitSignalGotFished(null);
    }

    protected override void Despawn()
    {
        if (!IsAlive)
            base.Despawn();
    }
}
