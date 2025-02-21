using Godot;
using Godot.Fish_the_fishes.Scripts;

[GlobalClass]
public partial class ScoredXPointsMax : DoneXTimes
{
    public override IAchievable.CheckTiming Timing { get { return IAchievable.CheckTiming.OnPointScored; } }

    public override string Stat { get { return Constants.MaxPointScored; } }
}
