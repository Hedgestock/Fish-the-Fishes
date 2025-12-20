using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using WaffleStock;

public partial class BossGiantSquid : Boss
{
    CollisionShape2D HurtBox;

    List<BossGiantSquidTentacle> Tentacles;

    float MaxTentacles;

    public override void _Ready()
    {
        base._Ready();

        HurtBox = (CollisionShape2D)GetNode("HurtBox");
        Tentacles = GetChildren().OfType<BossGiantSquidTentacle>().ToList();
        MaxTentacles = Tentacles.Count;

        foreach (var tentacle in Tentacles)
        {
            tentacle.Connect(BossGiantSquidTentacle.SignalName.GotCaught, Callable.From(
                () => LoseTentacle(tentacle)),
                (uint)ConnectFlags.OneShot);

            tentacle.Flip = Flip;

            tentacle.CatchRate = Tentacles.Count / (2 * MaxTentacles);
        }
    }

    protected override void PreparePass()
    {
        base.PreparePass();

        foreach (var tentacle in Tentacles)
        {
            tentacle.Flip = Flip;
        }
    }

    void LoseTentacle(BossGiantSquidTentacle tentacle)
    {
        Tentacles.Remove(tentacle);
        float catchRate = Mathf.Max(.1f, Tentacles.Count / (2 * MaxTentacles));
        foreach (var t in Tentacles)
        {
            t.CatchRate = catchRate;
        }

        if (HurtBox.Disabled && Tentacles.Count <= MaxTentacles / 2)
        {
            HurtBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
        }
    }

    public override IFishable GetCaughtBy(IFisher by)
    {
        foreach (var tentacle in Tentacles)
        {
            tentacle.IsCaught = true;
        }
        return base.GetCaughtBy(by);
    }
}
