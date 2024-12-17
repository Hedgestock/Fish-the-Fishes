using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class DoneXTimes : Achievement
{
    public virtual string Stat { get; }

    [Export]
    public virtual uint Threshold { get; set; }
    [Export]
    public virtual UserData.StatCategory Category { get; set; }
    [Export]
    public virtual Game.Mode Mode { get; set; }

    public override bool Predicate() { return UserData.GetStatistic(Category, Mode, Stat) >= Threshold; }

}
