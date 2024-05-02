using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Linq;

public partial class Compendium : CanvasLayer
{
    [Export]
    PackedScene FishEntry;

    [Export]
    VBoxContainer Fishes;

    [Export]
    VBoxContainer Trashes;

    [Export]
    VBoxContainer Biomes;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }

    private void AddFishEntry(string fishType)
    {
        FishCompendiumEntry newEntry = FishEntry.Instantiate<FishCompendiumEntry>();
        newEntry.TypeString = fishType;
        Fishes.AddChild(newEntry);
    }
}
