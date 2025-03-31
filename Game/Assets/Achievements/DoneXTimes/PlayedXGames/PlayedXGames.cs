using Godot;
using Godot.Fish_the_fishes.Scripts;

[GlobalClass]
public partial class PlayedXGames : DoneXTimes
{
    public override IAchievable.CheckTiming Timing { get { return IAchievable.CheckTiming.OnGameStart; } }

    public override string Stat { get { return Constants.TotalGamesPlayed; } }
}
