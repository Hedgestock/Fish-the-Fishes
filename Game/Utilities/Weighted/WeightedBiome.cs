using Godot;
using WaffleStock;
using System;

[GlobalClass]
public partial class WeightedBiome : WeightedItem
{
    [Export]
    public Constants.Biomes Biome { get; set; }
}
