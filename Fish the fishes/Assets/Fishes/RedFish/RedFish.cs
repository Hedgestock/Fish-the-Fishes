using Godot;
using System;

public partial class RedFish : Fish
{

    [Export]
    public float WaveAmplitude = 1;
    [Export]
    public float WaveAmplitudeVariation = 0.2f;
    [Export]
    public float WaveInversePeriod = 100;
    [Export]
    public float WaveInversePeriodVariation = 20;

    private float WaveActualPeriod = 0;
    private float WaveActualInverseAmplitude = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
        WaveActualPeriod = WaveAmplitude + (float)GD.RandRange(-WaveAmplitudeVariation, WaveAmplitudeVariation);
        WaveActualInverseAmplitude = WaveInversePeriod + (float) GD.RandRange(-WaveInversePeriodVariation, WaveInversePeriodVariation);

    }

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
        if (State != FishState.Alive) return;

		Vector2 tmp = new Vector2(1, (float) Math.Sin(Position.X/WaveActualInverseAmplitude) * WaveActualPeriod);
        Velocity = tmp.Normalized() * ActualSpeed;
    }

    public override void Kill()
    {
        base.Kill();
        GetNode<CollisionShape2D>("HitBox/CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

    private void _on_body_entered(Node body)
	{
        if (body is Fish && !(body is RedFish))
		{
			(body as Fish).Kill();
		}
    }
}
