using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Linq;

public partial class Compendium : CanvasLayer
{
    [Export]
    VBoxContainer Fishes;

    [Export]
    PackedScene FishEntry;

    [Export]
    VBoxContainer Trashes;

    [Export]
    PackedScene TrashEntry;

    [Export]
    VBoxContainer Biomes;

    public enum EntryType
    {
        Biome,
        Fish,
        Trash
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PopulateFishCompendium();
        PopulateTrashCompendium();
    }

    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }

    private void PopulateFishCompendium()
    {
        foreach (var entry in UserData.FishCompendium)
        {
            AddFishEntry(entry.Key);
        }

        var listOfExistingFishTypes = Enum.GetValues(typeof(Constants.Fishes))
            .Cast<Constants.Fishes>()
            .Select(v => v.ToString()).Except(UserData.FishCompendium.Keys);

        foreach (var fishType in listOfExistingFishTypes)
        {
            AddFishEntry(fishType);
        }
    }

    private void AddFishEntry(string fishType)
    {
        FishCompendiumEntry newEntry = FishEntry.Instantiate<FishCompendiumEntry>();
        newEntry.EntryKey = fishType;
        newEntry.EntryType = EntryType.Fish;
        Fishes.AddChild(newEntry);
    }

    private void PopulateTrashCompendium()
    {
        foreach (var entry in UserData.TrashCompendium)
        {
            AddTrashEntry(entry.Key);
        }

        var listOfExistingTrashTypes = Enum.GetValues(typeof(Constants.Trashes))
            .Cast<Constants.Trashes>()
            .Select(v => v.ToString()).Except(UserData.TrashCompendium.Keys);

        foreach (var trashType in listOfExistingTrashTypes)
        {
            AddTrashEntry(trashType);
        }
    }

    private void AddTrashEntry(string trashType)
    {
        TrashCompendiumEntry newEntry = TrashEntry.Instantiate<TrashCompendiumEntry>();
        newEntry.EntryKey = trashType;
        newEntry.EntryType = EntryType.Trash;
        Trashes.AddChild(newEntry);
    }
}
