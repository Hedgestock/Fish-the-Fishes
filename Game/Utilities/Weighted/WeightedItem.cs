using Godot;
using Godot.FishTheFishes;
using System;

public partial class WeightedItem : Resource
{
    [Export]
    public uint Weight = 100;

    public static WeightedItem ChooseFrom(WeightedItem[] list)
    {
        uint index = GD.Randi() % GetTotalWeight(list);

        uint currentWeight = 0;

        foreach (var item in list)
        {
            currentWeight += item.Weight;
            if (currentWeight > index)
                return item;
        }
        return null;
    }

    public static uint GetTotalWeight(WeightedItem[] list)
    {
        uint totalWeight = 0;

        foreach (var item in list)
        {
            totalWeight += item.Weight;
        }

        return totalWeight;
    }
}
