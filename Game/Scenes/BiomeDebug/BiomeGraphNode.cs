using Godot;
using WaffleStock;
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
            AddItem(weightedFish, WeightedFish.GetTotalWeight(Biome.Fishes.ToArray()));
        }

        foreach (WeightedTrash weightedTrash in Biome.Trashes)
        {
            AddItem(weightedTrash, WeightedTrash.GetTotalWeight(Biome.Trashes.ToArray())); ;
        }

        //FishDensity.Text += $"{Biome.TimeToSpawnFish}({Biome.TimeToSpawnFishDeviation})";
        //TrashDensity.Text += $"{Biome.TimeToSpawnTrash}({Biome.TimeToSpawnTrashDeviation})";
        //NextBiomeThreshold.Text += $"{Biome.ChangeBiomeThreshold}({Biome.ChangeBiomeThresholdDeviation})";
    }

    public void AddItem<ItemType>(WeightedItem<ItemType> weightedItem, uint maxWeight)
    {
        Label name = new();
        ProgressBar weight = new();

        name.SizeFlagsHorizontal = SizeFlags.ExpandFill;

        weight.Value = weightedItem.Weight;
        weight.MaxValue = maxWeight;
        weight.CustomMinimumSize = new Vector2(200, 0);
        weight.MouseFilter = MouseFilterEnum.Ignore;

        name.Text = weightedItem.Item.ToString();

        if (weightedItem is WeightedFish fish)
        {
            if (!UserData.BiomeCompendium.ContainsKey(Biome.ResourceName) || !UserData.FishCompendium.ContainsKey(name.Text))
                name.Text = "???";
            Fishes.AddChild(name);
            Fishes.AddChild(weight);
        }
        else if (weightedItem is WeightedTrash trash)
        {
            if (!UserData.BiomeCompendium.ContainsKey(Biome.ResourceName) || !UserData.TrashCompendium.ContainsKey(name.Text))
                name.Text = "???";
            Trashes.AddChild(name);
            Trashes.AddChild(weight);
        }
        else if (weightedItem is WeightedBiome biome)
        {
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
        GameManager.ChangeSceneToFile("res://Game/Scenes/Aquarium/Aquarium.tscn");
    }
}
