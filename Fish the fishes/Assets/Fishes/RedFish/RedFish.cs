using Godot;
using System;

public partial class RedFish : Fish
{

    [Export]
    public float wavePeriod = 1;
    [Export]
    public float wavePeriodVariation = 0.2f;
    [Export]
    public float waveInverseAmplitude = 100;
    [Export]
    public float waveInverseAmplitudeVariation = 20;

    private float waveActualPeriod = 0;
    private float waveActualInverseAmplitude = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
        waveActualPeriod = wavePeriod + (float)GD.RandRange(-wavePeriodVariation, wavePeriodVariation);
        waveActualInverseAmplitude = waveInverseAmplitude + (float) GD.RandRange(-waveInverseAmplitudeVariation, waveInverseAmplitudeVariation);

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
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
