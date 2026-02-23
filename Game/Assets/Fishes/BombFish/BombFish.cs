using Godot;
using Godot.Collections;
using System;
using System.Linq;
using WaffleStock;

public partial class BombFish : Fish
{
    [Export]
    Line2D Tail;
    [Export]
    GpuParticles2D Sparks;
    [Export]
    GpuParticles2D Sparks2;

    [Export]
    AnimatedSprite2D Explosion;

    [Export]
    Area2D ExplosionArea;


    uint Length = 10;
    uint SegmentLength = 10;
    private DateTime InstanciationTime = DateTime.Now;

    [Export]
    Curve AmplitudeCurve;
    int WaveAmplitude = 60;
    int WaveLength = 5;

    public override bool IsHuge => true; //This is not true, but makes it behave correctly when eaten

    public override void _Ready()
    {
        base._Ready();
        Explosion.Connect(AnimatedSprite2D.SignalName.AnimationFinished, Callable.From(QueueFree));
        Tail.Points = new Vector2[Length];
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Slither();
    }

    private void Slither()
    {
        Vector2[] tmp = new Vector2[Length];

        for (int i = 0; i < Length; i++)
        {
            tmp[i] = new Vector2(-SegmentLength * i,
                (float)Math.Sin((((DateTime.Now - InstanciationTime).TotalMilliseconds / 1000f) - (SegmentLength * (Length - i))) * WaveLength) * (WaveAmplitude * AmplitudeCurve.Sample((float)i / Length)));
        }

        Sparks.Position = tmp[Length - 1];
        Sparks2.Position = tmp[Length - 1];

        Tail.Points = tmp;
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

        Tail.Visible = false;
        GetNode<CollisionShape2D>("HurtBox").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        Explosion.Play();

        if (UserSettings.Vibrations) Input.VibrateHandheld(250);


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