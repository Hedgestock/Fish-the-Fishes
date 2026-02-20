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

        GameManager.Biome.Fishes = new(Enum.GetValues<Constants.Fishes>().Select(fish => new WeightedFish { Item = fish }));
        GameManager.Biome.Trashes = new(Enum.GetValues<Constants.Trashes>().Select(trash => new WeightedTrash { Item = trash }));

        foreach (var fish in GameManager.Biome.Fishes)
        {
            SpawnLine<Constants.Fishes> spawnLine = SpawnLineScene.Instantiate<SpawnLine<Constants.Fishes>>();
            spawnLine.Item = fish;
            FishesSettings.AddChild(spawnLine);
        }

        foreach (var trash in GameManager.Biome.Trashes)
        {
            SpawnLine<Constants.Trashes> spawnLine = SpawnLineScene.Instantiate<SpawnLine<Constants.Trashes>>();
            spawnLine.Item = trash;
            TrashesSettings.AddChild(spawnLine);
        }

        FoldingChanged += (bool isFolded) =>
        {
            if (isFolded)
            {
                SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
                SizeFlagsVertical = SizeFlags.ShrinkEnd;
                GameManager.Biome = GameManager.Biome;
            }
            else
            {
                SizeFlagsHorizontal = SizeFlags.Fill;
                SizeFlagsVertical = SizeFlags.Fill;
            }
        };
    }

    void SetAllFishes(bool spawn)
    {
        foreach (var spawnLine in FishesSettings.GetChildren().OfType<SpawnLine<Constants.Fishes>>())
        {
            spawnLine.CheckBox.ButtonPressed = spawn;
        }
    }
    void SetAllTrashes(bool spawn)
    {
        foreach (var spawnLine in TrashesSettings.GetChildren().OfType<SpawnLine<Constants.Trashes>>())
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