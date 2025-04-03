using Godot;
using WaffleStock;
using System;

[GlobalClass]
public partial class WeightedFish : WeightedItem
{
    [Export]
    public Constants.Fishes Fish { get; set; }
}
