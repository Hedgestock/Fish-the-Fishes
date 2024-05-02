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

        var listOfExistingFishTypes = AppDomain.CurrentDomain.GetAssemblies()
         .SelectMany(domainAssembly => domainAssembly.GetTypes())
         .Where(type => type.IsSubclassOf(typeof(Fish))
         ).Select(type => type.Name).Except(UserData.FishCompendium.Keys);

        foreach (var fishType in listOfExistingFishTypes)
        {
            AddFishEntry(fishType);
        }
    }

    private void AddFishEntry(string fishType)
    {
        FishCompendiumEntry newEntry = FishEntry.Instantiate<FishCompendiumEntry>();
        newEntry.TypeString = fishType;
        Fishes.AddChild(newEntry);
    }

    private void PopulateTrashCompendium()
    {
        foreach (var entry in UserData.TrashCompendium)
        {
            AddTrashEntry(entry.Key);
        }

        var listOfExistingTrashTypes = AppDomain.CurrentDomain.GetAssemblies()
         .SelectMany(domainAssembly => domainAssembly.GetTypes())
         .Where(type => type.IsSubclassOf(typeof(Trash))
         ).Select(type => type.Name).Except(UserData.TrashCompendium.Keys);

        foreach (var trashType in listOfExistingTrashTypes)
        {
            AddTrashEntry(trashType);
        }
    }

    private void AddTrashEntry(string trashType)
    {
        TrashCompendiumEntry newEntry = TrashEntry.Instantiate<TrashCompendiumEntry>();
        newEntry.TypeString = trashType;
        Trashes.AddChild(newEntry);
    }
}
