using Godot;
using System;
using System.Text.RegularExpressions;

public partial class BiomeGraphNode : GraphNode
{
    [Export]
    VBoxContainer Fishes;
    
    [Export]
    VBoxContainer Trashes;
    
    [Export]
    VBoxContainer Biomes;

    [Export]
    PackedScene WeightedItemControl;

    public override void _Ready()
    {
        base._Ready();

        //SetSlot(0, true, 0, new Color(1, 1, 1), true, 0, new Color(0, 0, 0));
    }

    public void AddFish(WeightedFish fish)
    {
        WeightedItemControl Item = WeightedItemControl.Instantiate<WeightedItemControl>();

        Item.ItemName.Text = fish.Fish.ToString();
        Item.WeighInput.Text = fish.Weight.ToString();

        Fishes.AddChild(Item);
    }

    public void AddTrash(WeightedTrash trash)
    {
        WeightedItemControl Item = WeightedItemControl.Instantiate<WeightedItemControl>();

        Item.ItemName.Text = trash.Trash.ToString();
        Item.WeighInput.Text = trash.Weight.ToString();

        Trashes.AddChild(Item);
    }

    public void AddBiome(WeightedBiome biome)
    {
        WeightedItemControl Item = WeightedItemControl.Instantiate<WeightedItemControl>();

        Item.ItemName.Text = biome.Biome.ToString();
        Item.WeighInput.Text = biome.Weight.ToString();

        Biomes.AddChild(Item);
    }

}
