using Godot;
using System;

public partial class RedFish : Fish
{

    [Export]
    public float waveAmplitude = 1;
    [Export]
    public float waveAmplitudeVariation = 0.2f;
    [Export]
    public float waveInversePeriod = 100;
    [Export]
    public float waveInversePeriodVariation = 20;

    private float waveActualPeriod = 0;
    private float waveActualInverseAmplitude = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
        waveActualPeriod = waveAmplitude + (float)GD.RandRange(-waveAmplitudeVariation, waveAmplitudeVariation);
        waveActualInverseAmplitude = waveInversePeriod + (float) GD.RandRange(-waveInversePeriodVariation, waveInversePeriodVariation);

    }

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
        if (state != State.Alive) return;

		Vector2 tmp = new Vector2(1, (float) Math.Sin(Position.X/waveActualInverseAmplitude) * waveActualPeriod);
        LinearVelocity = tmp.Normalized() * actualSpeed;
    }

    private void _on_body_entered(Node body)
	{
        if (body is Fish && !(body is RedFish))
		{
			(body as Fish).Kill();
		}
    }
}
