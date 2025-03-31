using Godot;
using Wafflestock;
using System;

[GlobalClass]
public partial class WeightedBiome : WeightedItem
{
    [Export]
    public Constants.Biomes Biome { get; set; }
}
