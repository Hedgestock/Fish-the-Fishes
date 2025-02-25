using Godot;
using System;
using System.Text.RegularExpressions;
using static Godot.Fish_the_fishes.Scripts.Constants;

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

    public Biome Biome;

    public override void _Ready()
    {
        base._Ready();

        Title = Biome.ResourceName;

        foreach (WeightedFish weightedFish in Biome.Fishes)
        {
            AddFish(weightedFish);
        }

        foreach (WeightedTrash weightedTrash in Biome.Trashes)
        {
            AddTrash(weightedTrash);
        }

        StyleBoxTexture test = new();
        test.Texture = Biome.Background;
        AddThemeStyleboxOverride("panel",test);
        AddThemeStyleboxOverride("panel_selected",test);

    }

    public void AddFish(WeightedFish fish)
    {
        WeightedItemControl Item = WeightedItemControl.Instantiate<WeightedItemControl>();

        Item.ItemName.Text = fish.Fish.ToString();
        Item.Weight.Text = fish.Weight.ToString();

        Fishes.AddChild(Item);
    }

    public void AddTrash(WeightedTrash trash)
    {
        WeightedItemControl Item = WeightedItemControl.Instantiate<WeightedItemControl>();

        Item.ItemName.Text = trash.Trash.ToString();
        Item.Weight.Text = trash.Weight.ToString();

        Trashes.AddChild(Item);
    }

    public void AddBiome(WeightedBiome biome)
    {
        WeightedItemControl Item = WeightedItemControl.Instantiate<WeightedItemControl>();

        Item.ItemName.Text = biome.Biome.ToString();
        Item.Weight.Text = biome.Weight.ToString();

        AddChild(Item);
    }

}
