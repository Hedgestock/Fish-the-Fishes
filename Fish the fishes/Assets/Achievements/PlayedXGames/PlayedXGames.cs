using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayedXGames : Achievement
{
    public override IAchievable.CheckTiming Timing { get { return IAchievable.CheckTiming.OnGameStart; } }

    [Export]
    public uint GamesThreshold = 0;

    [Export]
    public Game.Mode Mode { get; set; }

    [ExportGroup("Compendium")]
    [Export]
    public override string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public override string CompendiumDescription { get; set; }

    public override bool Predicate() {
        if (Mode == Game.Mode.Menu)
            return UserData.Statistics.GetValueOrDefault(Constants.TotalGamesPlayed) > GamesThreshold;
        return false;
    }
}
