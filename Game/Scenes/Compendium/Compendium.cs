using Godot;
using Wafflestock;
using System;
using System.Linq;
using static Wafflestock.Constants;

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

    [Export]
    PackedScene BiomeEntry;

    [Export]
    VBoxContainer Achievements;

    [Export]
    PackedScene AchievementEntry;

    public enum EntryType
    {
        Biome,
        Fish,
        Trash,
        Achievement
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PopulateFishCompendium();
        PopulateTrashCompendium();
        PopulateBiomeCompendium();
        PopulateAchievementCompendium();
    }

    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
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

    private void PopulateBiomeCompendium()
    {
        foreach (var entry in UserData.BiomeCompendium)
        {
            AddBiomeEntry(entry.Key);
        }

        var listOfExistingBiomeTypes = Enum.GetValues(typeof(Constants.Biomes))
            .Cast<Constants.Biomes>()
            .Select(v => v.ToString()).Except(UserData.BiomeCompendium.Keys);

        foreach (var biomeType in listOfExistingBiomeTypes)
        {
            AddBiomeEntry(biomeType);
        }
    }

    private void AddBiomeEntry(string biomeType)
    {
        BiomeCompendiumEntry newEntry = BiomeEntry.Instantiate<BiomeCompendiumEntry>();
        newEntry.EntryKey = biomeType;
        newEntry.EntryType = EntryType.Biome;
        Biomes.AddChild(newEntry);
    }

    private void PopulateAchievementCompendium()
    {
        foreach (var entry in UserData.Achievements)
        {
            AddAchievementEntry(entry.Key);
        }

        var listOfLockedAchievements = AchievementsManager.Instance.AchievementsList.Select(a => a.ResourcePath).Except(UserData.Achievements.Keys);

        foreach (var achievementPath in listOfLockedAchievements)
        {
            AddAchievementEntry(achievementPath);
        }
    }

    private void AddAchievementEntry(string achievementPath)
    {
        AchievementCompendiumEntry newEntry = AchievementEntry.Instantiate<AchievementCompendiumEntry>();
        newEntry.EntryKey = achievementPath;
        newEntry.EntryType = EntryType.Achievement;
        Achievements.AddChild(newEntry);
    }

}
