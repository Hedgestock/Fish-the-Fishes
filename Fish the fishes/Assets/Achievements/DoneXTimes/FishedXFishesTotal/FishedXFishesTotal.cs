using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class FishedXFishesTotal : DoneXTimes
{
    public override IAchievable.CheckTiming Timing { get { return IAchievable.CheckTiming.OnFishFished; } }

    public override string Stat { get { return Constants.TotalFishedFishes; } }
}
