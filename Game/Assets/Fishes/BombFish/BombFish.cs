using Godot;
using Godot.Collections;
using System;
using System.Linq;
using WaffleStock;

public partial class BombFish : Fish
{
    //[Export]
    //public Array<Constants.Fishes> ImmuneToExplosion = new Array<Constants.Fishes>();

    [Export]
    AnimatedSprite2D Explosion;

    [Export]
    Area2D ExplosionArea;

    public override void _Ready()
    {
        base._Ready();
        Explosion.Connect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(QueueFree));
    }

    public override bool GetCaughtBy(IFisher by)
    {
        Kill();
        return false;
    }

    public override void Kill()
    {
        if (CantGetCaught) return;
        CantGetCaught = true;
        IsCaught = true;
        Velocity = Vector2.Zero;
        base.Kill();
        Explosion.Play();
        GetTree().CreateTimer(.2).Timeout += () =>
        {
            foreach (var fish in ExplosionArea.GetOverlappingBodies().OfType<Fish>())
            {
                fish.Kill();
            }
        };
    }
}