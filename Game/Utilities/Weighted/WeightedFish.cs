using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

[GlobalClass]
public partial class WeightedFish : WeightedItem
{
    [Export]
    public Constants.Fishes Fish { get; set; }
}
