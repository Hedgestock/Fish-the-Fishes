using Godot;
using WaffleStock;

[GlobalClass]
public partial class FishedXFishesTotal : DoneXTimes
{
    public override IAchievable.CheckTiming Timing { get { return IAchievable.CheckTiming.OnFishFished; } }

    public override string Stat { get { return Constants.TotalFishedFishes; } }
}
