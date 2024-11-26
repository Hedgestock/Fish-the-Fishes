using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class StartFishing : Achievement
{
    public override IAchievable.CheckTiming Timing { get { return IAchievable.CheckTiming.OnGameStart; } }

    [ExportGroup("Compendium")]
    [Export]
    public override string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public override string CompendiumDescription { get; set; }

    public override bool Predicate() { return UserData.Statistics.GetValueOrDefault(Constants.TotalGamesPlayed) > 0; }
}
