using Godot;
using Godot.Collections;
using System;
using System.Linq;
using WaffleStock;

public partial class TrainingUI : FoldableContainer
{
    [Export]
    PackedScene SpawnLineFishScene;
    [Export]
    PackedScene SpawnLineTrashScene;

    [Export]
    TabContainer Tabs;
    [Export]
    Container FishesSettings;
    [Export]
    Container TrashesSettings;

    public override void _Ready()
    {
        base._Ready();

        Tabs.SetTabTitle(0, Tr("GENERAL"));
        Tabs.SetTabTitle(1, Tr("FISHES"));
        Tabs.SetTabTitle(2, Tr("TRASHES"));

        GameManager.Biome.Fishes = new(Enum.GetValues<Constants.Fishes>().Select(fish => new WeightedFish { Item = fish }));
        GameManager.Biome.Trashes = new(Enum.GetValues<Constants.Trashes>().Select(trash => new WeightedTrash { Item = trash }));

        foreach (var fish in GameManager.Biome.Fishes)
        {
            SpawnLine<Constants.Fishes> spawnLine = SpawnLineFishScene.Instantiate<SpawnLineFish>();
            spawnLine.Item = fish;
            FishesSettings.AddChild(spawnLine);
        }

        foreach (var trash in GameManager.Biome.Trashes)
        {
            SpawnLine<Constants.Trashes> spawnLine = SpawnLineTrashScene.Instantiate<SpawnLineTrash>();
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