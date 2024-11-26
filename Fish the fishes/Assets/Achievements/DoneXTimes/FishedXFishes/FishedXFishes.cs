using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class FishedXFishes : DoneXTimes
{
    public override IAchievable.CheckTiming Timing { get { return IAchievable.CheckTiming.OnFishFished; } }
    [Export]
    public override uint Threshold { get; set; }
    [Export]
    public override UserData.StatCategory Category { get; set; }
    [Export]
    public override Game.Mode Mode { get; set; }

    [ExportGroup("Compendium")]
    [Export]
    public override string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public override string CompendiumDescription { get; set; }

    public override bool Predicate() { return UserData.GetStatistic(Category, Mode, Constants.TotalFishedFishes) >= Threshold; }
}
