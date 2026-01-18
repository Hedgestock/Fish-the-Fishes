using Godot;
using Godot.Collections;
using System;
using System.Linq;
using WaffleStock;

public partial class Test : FoldableContainer
{
    [Export]
    PackedScene SpawnLineScene;

    [Export]
    Container BiomeSettings;

    public override void _Ready()
    {
        base._Ready();

        GameManager.Biome.Fishes = new(Enum.GetValues<Constants.Fishes>().Select(f => new WeightedFish { Fish = f }));
        GameManager.Biome.Trashes = new(Enum.GetValues<Constants.Trashes>().Select(t => new WeightedTrash { Trash = t }));

        foreach (var fish in GameManager.Biome.Fishes)
        {
            SpawnLine spawnLine = SpawnLineScene.Instantiate<SpawnLine>();
            spawnLine.Item = fish;
            BiomeSettings.AddChild(spawnLine);
        }

        FoldingChanged += (bool isFolded) =>
        {
            if (isFolded)
                SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
            else
                SizeFlagsHorizontal = SizeFlags.Fill;
        };
    }

    void SetAllFishes(bool spawn)
    {
        foreach (var spawnLine in BiomeSettings.GetChildren().OfType<SpawnLine>())
        {
            spawnLine.CheckBox.ButtonPressed = spawn;
        }
    }
}