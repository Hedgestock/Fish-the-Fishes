using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

[GlobalClass]
public abstract partial class Achievement : Resource, IAchievable
{
    public abstract IAchievable.CheckTiming Timing { get; }

    public abstract string CompendiumName { get; set; }
    public abstract string CompendiumDescription { get; set; }

    public abstract bool Predicate();
}
