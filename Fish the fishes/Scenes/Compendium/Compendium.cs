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
        int entryCount = 0;
        foreach (var entry in UserData.Instance.Compendium)
        {
            FishCompendiumEntry newEntry = FishEntry.Instantiate<FishCompendiumEntry>();
            newEntry.FishTypeString = entry.Key;
            Fishes.AddChild(newEntry);
            entryCount++;
        }

        var listOfExistingFishes = AppDomain.CurrentDomain.GetAssemblies()
         .SelectMany(domainAssembly => domainAssembly.GetTypes())
         .Where(type => type.IsSubclassOf(typeof(Fish))
         ).ToArray();

        for (; entryCount < listOfExistingFishes.Length; entryCount++)
        {
            FishCompendiumEntry newEntry = FishEntry.Instantiate<FishCompendiumEntry>();
            Fishes.AddChild(newEntry);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void GoToHome()
    {
        GetNode<GameManager>("/root/GameManager").ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }
}
