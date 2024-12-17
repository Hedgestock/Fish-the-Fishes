using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Achievement : Resource, IAchievable
{
    public virtual IAchievable.CheckTiming Timing { get; }
    [Export]
    public virtual string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public virtual string CompendiumDescription { get; set; }

    public virtual bool Predicate() { return false; }
}
