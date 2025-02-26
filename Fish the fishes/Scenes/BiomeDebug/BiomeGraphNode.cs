using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Linq;
using System.Xml.Linq;

public partial class BiomeGraphNode : GraphNode
{
    [Export]
    GridContainer Fishes;
    [Export]
    GridContainer Trashes;

    [Export]
    Label FishDensity;
    [Export]
    Label TrashDensity;
    [Export]
    Label NextBiomeThreshold;

    public Biome Biome;

    public override void _Ready()
    {
        base._Ready();


        Title = Biome.CompendiumName;
        if (!UserData.BiomeCompendium.ContainsKey(Title))
            Title = "???";

        StyleBoxTexture test = new();
        test.Texture = Biome.Background;
        AddThemeStyleboxOverride("panel", test);
        AddThemeStyleboxOverride("panel_selected", test);

        foreach (WeightedFish weightedFish in Biome.Fishes)
        {
            AddItem(weightedFish, WeightedItem.GetTotalWeight(Biome.Fishes.ToArray()));
        }

        foreach (WeightedTrash weightedTrash in Biome.Trashes)
        {
            AddItem(weightedTrash, WeightedItem.GetTotalWeight(Biome.Trashes.ToArray())); ;
        }

        FishDensity.Text += $"{Biome.TimeToSpawnFish}({Biome.TimeToSpawnFishDeviation})";
        TrashDensity.Text += $"{Biome.TimeToSpawnTrash}({Biome.TimeToSpawnTrashDeviation})";
        NextBiomeThreshold.Text += $"{Biome.ChangeBiomeThreshold}({Biome.ChangeBiomeThresholdDeviation})";
    }

    public void AddItem(WeightedItem item, uint maxWeight)
    {
        Label name = new();
        ProgressBar weight = new();

        name.SizeFlagsHorizontal = SizeFlags.ExpandFill;

        weight.Value = item.Weight;
        weight.MaxValue = maxWeight;
        weight.CustomMinimumSize = new Vector2(200, 0);
        weight.MouseFilter = MouseFilterEnum.Ignore;
        if (item is WeightedFish fish)
        {
            name.Text = fish.Fish.ToString();
            if (!UserData.BiomeCompendium.ContainsKey(Biome.ResourceName) || !UserData.FishCompendium.ContainsKey(name.Text))
                name.Text = "???";
            Fishes.AddChild(name);
            Fishes.AddChild(weight);
        }
        else if (item is WeightedTrash trash)
        {
            name.Text = trash.Trash.ToString();
            if (!UserData.BiomeCompendium.ContainsKey(Biome.ResourceName) || !UserData.TrashCompendium.ContainsKey(name.Text))
                name.Text = "???";
            Trashes.AddChild(name);
            Trashes.AddChild(weight);
        }
        else if (item is WeightedBiome biome)
        {

            name.Text = biome.Biome.ToString();
            if (!UserData.BiomeCompendium.ContainsKey(name.Text))
                name.Text = "???";

            HBoxContainer container = new HBoxContainer();
            container.AddChild(name);
            container.AddChild(weight);
            AddChild(container);
        }

    }

    private void LaunchAquarium()
    {
        GameManager.Biome = Biome;
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Aquarium/Aquarium.tscn");
    }
}
