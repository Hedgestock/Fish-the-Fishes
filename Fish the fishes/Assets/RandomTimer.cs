using Godot;
using System;

public partial class RandomTimer : Timer
{
	[Export]
	public double WaitTimeAverage = 1;
	[Export]
	public double Deviation = 0.2;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
		WaitTime = WaitTimeAverage + GD.RandRange(-Deviation, Deviation);

		Connect(Timer.SignalName.Timeout, Callable.From(() => WaitTime = WaitTimeAverage + GD.RandRange(-Deviation, Deviation)));
    }

    public new void Start(double timeSec = -1.0)
    {
		if (timeSec > 0)
		{
			WaitTimeAverage = timeSec;
		}

		WaitTime = WaitTimeAverage + GD.RandRange(-Deviation, Deviation);

		base.Start();
	}
}
