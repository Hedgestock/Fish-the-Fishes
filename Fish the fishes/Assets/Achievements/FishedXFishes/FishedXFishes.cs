using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class FishedXFishes : Achievement
{
    public override IAchievable.CheckTiming Timing { get { return IAchievable.CheckTiming.OnFishFished; } }

    [Export]
    public uint FishesThreshold = 0;

    [ExportGroup("Compendium")]
    [Export]
    public override string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public override string CompendiumDescription { get; set; }

    public override bool Predicate() { return UserData.Statistics.GetValueOrDefault(Constants.TotalFishedFishes) >= FishesThreshold; }
}
