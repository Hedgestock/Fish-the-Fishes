using Godot;
using WaffleStock;
using System;

[GlobalClass]
public partial class WeightedBiome : WeightedItem<Constants.Biomes>
{
    [Export]
    new public Constants.Biomes Item
    {
        get { return base.Item; }
        set { base.Item = value; }
    }
}
