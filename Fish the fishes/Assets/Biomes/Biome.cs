using Godot;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Linq;

[GlobalClass]
public partial class Biome : Resource, IDescriptible
{

    [Export]
    public Array<WeightedFish> Fishes;

    [Export]
    public Array<WeightedTrash> Trashes;

    [Export]
    public Array<WeightedBiome> FollowupBiomes;

    [ExportGroup("Compendium")]
    [Export]
    public string CompendiumName { get; set; }
    [Export(PropertyHint.MultilineText)]
    public string CompendiumDescription { get; set; }

    [ExportGroup("Ambiance")]
    [Export]
    public Texture2D Background;

    public static string GetRandomPathFrom(Array<WeightedFish> list)
    {
        string FishType = (ChooseFrom(list.ToArray()) as WeightedFish).Fish.ToString();
        return $"{Constants.FishesFolder}{FishType}/{FishType}.tscn";
    }

    public static string GetRandomPathFrom(Array<WeightedTrash> list)
    {
        string TrashType = (ChooseFrom(list.ToArray()) as WeightedTrash).Trash.ToString();
        return $"{Constants.TrashesFolder}{TrashType}/{TrashType}.tscn";
    }

    public static string GetRandomPathFrom(Array<WeightedBiome> list)
    {
        string BiomeType = (ChooseFrom(list.ToArray()) as WeightedBiome).Biome.ToString();
        return $"{Constants.BiomesFolder}{BiomeType}/{BiomeType}.tres";
    }

    public static WeightedItem ChooseFrom(WeightedItem[] list)
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
                return item;
        }
        return null;
    }
}
