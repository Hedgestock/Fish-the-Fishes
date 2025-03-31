using Godot;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Linq;

[GlobalClass]
public partial class Biome : Resource, IDescriptible
{
    [Export]
    public float TimeToSpawnFish = 1;
    [Export]
    public float TimeToSpawnFishDeviation = 0.2f;

    [Export]
    public float TimeToSpawnTrash = 3;
    [Export]
    public float TimeToSpawnTrashDeviation = 0.5f;

    [Export]
    public int ChangeBiomeThreshold = 50;

    [Export]
    public int ChangeBiomeThresholdDeviation = 5;

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
        string FishType = (WeightedItem.ChooseFrom(list.ToArray()) as WeightedFish).Fish.ToString();
        return $"{Constants.FishesFolder}{FishType}/{FishType}.tscn";
    }

    public static string GetRandomPathFrom(Array<WeightedTrash> list)
    {
        string TrashType = (WeightedItem.ChooseFrom(list.ToArray()) as WeightedTrash).Trash.ToString();
        return $"{Constants.TrashesFolder}{TrashType}/{TrashType}.tscn";
    }

    public static string GetRandomPathFrom(Array<WeightedBiome> list)
    {
        string BiomeType = (WeightedItem.ChooseFrom(list.ToArray()) as WeightedBiome).Biome.ToString();
        return $"{Constants.BiomesFolder}{BiomeType}/{BiomeType}.tres";
    }
}
