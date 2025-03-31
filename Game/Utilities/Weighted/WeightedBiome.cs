using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

[GlobalClass]
public partial class WeightedBiome : WeightedItem
{
    [Export]
    public Constants.Biomes Biome { get; set; }
}
