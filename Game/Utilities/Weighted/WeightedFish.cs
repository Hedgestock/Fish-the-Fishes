using Godot;
using WaffleStock;
using System;

[GlobalClass]
public partial class WeightedFish : WeightedItem<Constants.Fishes>
{
    [Export]
    new public Constants.Fishes Item
    {
        get { return base.Item; }
        set { base.Item = value; }
    }
}
