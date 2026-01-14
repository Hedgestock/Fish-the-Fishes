using Godot;
using Godot.Collections;
using System;
using System.Linq;
using WaffleStock;

public partial class BombFish : Fish
{
        [Export]
    AnimatedSprite2D Explosion;

    [Export]
    Area2D ExplosionArea;

    public override bool IsHuge => true; //This is not true, but makes it behave correctly when eaten

    public override void _Ready()
    {
        base._Ready();
        Explosion.Connect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(QueueFree));
    }

    public override void GetCaughtBy(IFisher fisher)
    {
        Kill();
    }

    public override void Kill()
    {
        if (CantGetCaught) return;
        CantGetCaught = true;
        IsCaught = true;
        Velocity = Vector2.Zero;
        base.Kill();
        Explosion.Play();

        FishingLine Line = GetNode<FishingLine>("../FishingLine");
        if (ExplosionArea.OverlapsArea(Line.HitBox))
            Line.GetHit(FishingLine.DamageType.Explosion);

        GetTree().CreateTimer(.2).Timeout += () =>
        {
            foreach (var fish in ExplosionArea.GetOverlappingBodies().OfType<Fish>())
            {
                fish.Kill();
            }
        };
    }
}