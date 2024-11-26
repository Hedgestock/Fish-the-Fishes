using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public abstract partial class DoneXTimes : Achievement
{
    public abstract uint Threshold { get; set; }
    public abstract UserData.StatCategory Category { get; set; }
    public abstract Game.Mode Mode { get; set; }
}
