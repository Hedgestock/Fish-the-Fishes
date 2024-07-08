using Godot;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using System;


public partial class RedFish : Fish
{
    [Export]
    public Array<Constants.Fishes> ImmuneToRed = new Array<Constants.Fishes>();
    
    [ExportGroup("Behaviour")]
    [Export]
    public float WaveAmplitude = 1;
    [Export]
    public float WaveAmplitudeDeviation = 0.2f;
    [Export]
    public float WaveInversePeriod = 100;
    [Export]
    public float WaveInversePeriodDeviation = 20;

    private float WaveActualPeriod = 0;
    private float WaveActualInverseAmplitude = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        WaveActualPeriod = (float)GD.Randfn(WaveAmplitude, WaveAmplitudeDeviation);
        WaveActualInverseAmplitude = (float)GD.Randfn(WaveInversePeriod, WaveInversePeriodDeviation);
        GD.Print($"{GetType()} {typeof(RedFish)} {Constants.Fishes.RedFish} {GetType() == typeof(RedFish)} {GetType().ToString() == Constants.Fishes.RedFish.ToString()}");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!IsActionable) return;

        Vector2 tmp = new Vector2(1, (float)Math.Sin(Position.X / WaveActualInverseAmplitude) * WaveActualPeriod);
        Velocity = tmp.Normalized() * ActualSpeed * (Flip ? -1 : 1);
    }

    public override void Kill()
    {
        base.Kill();
        GetNode<CollisionShape2D>("HitBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    private void _on_body_entered(Node body)
    {
        if (body is Fish && !CheckImmunity(ImmuneToRed, body.GetType()))
        {
            (body as Fish).Kill();
        }
    }
}
