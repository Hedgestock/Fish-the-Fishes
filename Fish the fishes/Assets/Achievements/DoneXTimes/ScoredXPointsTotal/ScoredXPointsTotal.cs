using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

[GlobalClass]
public partial class ScoredXPointsTotal : DoneXTimes
{
    public override IAchievable.CheckTiming Timing { get { return IAchievable.CheckTiming.OnPointScored; } }

    public override string Stat { get { return Constants.TotalPointsScored; } }
}
