using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WaffleStock;

public partial class BossGiantSquid : Boss
{
    CollisionShape2D HurtBox;

    List<BossGiantSquidTentacle> Tentacles;

    float MaxTentacles;

    DateTime GotHurt = DateTime.MinValue;

    public override bool IsActionable => base.IsActionable && (DateTime.Now - GotHurt).TotalSeconds > 3;

    public override void _Ready()
    {
        foreach (var eye in GetChildren().OfType<BossGiantSquidEye>())
        {
            eye.Connect(BossGiantSquidEye.SignalName.Closing, Callable.From(EyePoked));
        }

        HurtBox = (CollisionShape2D)GetNode("HurtBox");
        Tentacles = GetChildren().OfType<BossGiantSquidTentacle>().ToList();
        MaxTentacles = Tentacles.Count;

        base._Ready();

        foreach (var tentacle in Tentacles)
        {
            tentacle.Connect(Fish.SignalName.GotFished, Callable.From(
                (Node by) => LoseTentacle(tentacle)),
                (uint)ConnectFlags.OneShot);

            tentacle.Flip = Flip;

            tentacle.CatchRate = Tentacles.Count / (2 * MaxTentacles);

            tentacle.IsInDisplay = IsInDisplay;
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
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity *= 1.0f - (float)ProjectSettings.GetSetting("physics/2d/default_linear_damp") * (float)delta;

        if (Velocity.Length() < ActualSpeed * .05)
            Push();
    }

    protected override void SetSpeed()
    {
        base.SetSpeed();
        ActualSpeed = Mathf.Max(MinSpeed, ActualSpeed * (Tentacles.Count / MaxTentacles));
    }

    private void Push()
    {
        if (IsActionable)
        {
            SetSpeed();
            Velocity = TravelAxis * ActualSpeed;
        }
    }

    void LoseTentacle(BossGiantSquidTentacle tentacle)
    {
        Tentacles.Remove(tentacle);
        float catchRate = Mathf.Max(.1f, Tentacles.Count / (3 * MaxTentacles));
        foreach (var t in Tentacles)
        {
            t.CatchRate = catchRate;
        }

        if (HurtBox.Disabled && Tentacles.Count <= MaxTentacles / 2)
        {
            HurtBox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
        }
    }

    void EyePoked()
    {
        Velocity /= 2;
        GotHurt = DateTime.Now;
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
