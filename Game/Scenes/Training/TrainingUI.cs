using Godot;
using Godot.Collections;
using System;
using System.Linq;
using WaffleStock;

public partial class TrainingUI : FoldableContainer
{
    [Export]
    PackedScene SpawnLineScene;

    [Export]
    Container FishesSettings;

    [Export]
    Container TrashesSettings;

    public override void _Ready()
    {
        base._Ready();

        GameManager.Biome.Fishes = new(Enum.GetValues<Constants.Fishes>().Select(f => new WeightedFish { Fish = f }));
        GameManager.Biome.Trashes = new(Enum.GetValues<Constants.Trashes>().Select(t => new WeightedTrash { Trash = t }));

        foreach (var fish in GameManager.Biome.Fishes)
        {
            SpawnLine spawnLine = SpawnLineScene.Instantiate<SpawnLine>();
            spawnLine.Item = fish;
            FishesSettings.AddChild(spawnLine);
        }

        foreach (var fish in GameManager.Biome.Fishes)
        {
            SpawnLine spawnLine = SpawnLineScene.Instantiate<SpawnLine>();
            spawnLine.Item = fish;
            FishesSettings.AddChild(spawnLine);
        }

        FoldingChanged += (bool isFolded) =>
        {
            if (isFolded)
            {
                SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
                GameManager.Biome = GameManager.Biome;
            }
            else
                SizeFlagsHorizontal = SizeFlags.Fill;
        };
    }

    void SetAllFishes(bool spawn)
    {
        foreach (var spawnLine in FishesSettings.GetChildren().OfType<SpawnLine>())
        {
            spawnLine.CheckBox.ButtonPressed = spawn;
        }
    }
    void SetAllTrashes(bool spawn)
    {
        foreach (var spawnLine in TrashesSettings.GetChildren().OfType<SpawnLine>())
        {
            spawnLine.CheckBox.ButtonPressed = spawn;
        }
    }

    void SetTimeToSpawnFish(float time)
    {
        GameManager.Biome.TimeToSpawnFish = time;
    }

    void SetTimeToSpawnFishDeviation(float timeDeviation)
    {
        GameManager.Biome.TimeToSpawnFishDeviation = timeDeviation;
    }

    void SetTimeToSpawnTrash(float time)
    {
        GameManager.Biome.TimeToSpawnTrash = time;
    }

    void SetTimeToSpawnTrashDeviation(float timeDeviation)
    {
        GameManager.Biome.TimeToSpawnTrashDeviation = timeDeviation;
    }
}