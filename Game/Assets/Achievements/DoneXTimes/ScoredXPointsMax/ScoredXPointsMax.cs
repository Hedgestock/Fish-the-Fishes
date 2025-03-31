using Godot;
using Wafflestock;

[GlobalClass]
public partial class ScoredXPointsMax : DoneXTimes
{
    public override IAchievable.CheckTiming Timing { get { return IAchievable.CheckTiming.OnPointScored; } }

    public override string Stat { get { return Constants.MaxPointScored; } }
}
