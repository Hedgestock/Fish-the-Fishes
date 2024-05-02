using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class Biome : Resource
{
    [Export]
    public Array<WeightedItem> Fishes;

    [Export]
    public Array<WeightedItem> Trashes;

    [Export]
    public Array<WeightedItem> FollowupBiomes;

    [ExportGroup("Ambiance")]
    [Export]
    public Texture2D Background;

    public static GodotObject ChooseFrom(Array<WeightedItem> list)
    {
        uint totalWeight = 0;
        foreach (var item in list)
        {
            totalWeight += item.Weight;
        }

        uint index = GD.Randi() % totalWeight;

        uint currentWeight = 0;

        foreach (var item in list)
        {
            currentWeight += item.Weight;
            if (currentWeight > index)
                return item.Item;
        }
        return null;
    }
}
