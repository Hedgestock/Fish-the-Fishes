using Godot;
using WaffleStock;
using System;
using System.Linq;
using static WaffleStock.Constants;

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

    [Export]
    OptionButton TabButton;
    [Export]
    TabContainer TabContainer;

    [Export]
    OptionButton FilterButton;

    public enum EntryType
    {
        Fish,
        Trash,
        Biome,
        Achievement
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PopulateFishCompendium();
        PopulateTrashCompendium();
        PopulateBiomeCompendium();
        PopulateAchievementCompendium();

        // TODO: Actually look at the tabs
        foreach (EntryType entryType in Enum.GetValues<EntryType>())
        {
            TabButton.AddItem(entryType.ToString(), (int)entryType);
        }

        Biomes[] biomes = Enum.GetValues<Biomes>();
        foreach (Biomes biome in biomes)
        {
            FilterButton.AddItem(biome.ToString(), (int)biome);
        }
        FilterButton.AddItem("No filter", biomes.Length);
        FilterButton.Selected = biomes.Length;
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

    private void ChangeTab(int tabIndex)
    {
        TabContainer.CurrentTab = tabIndex;
        FilterButton.Visible = tabIndex <= (int)EntryType.Trash;
    }

    private void Filter(int biomeIndex)
    {
        if (biomeIndex >= Enum.GetValues<Biomes>().Length)
        {
            foreach (var fishEntry in Fishes.GetChildren().OfType<FishCompendiumEntry>())
            {
                fishEntry.Visible = true;
            }
            foreach (var trashEntry in Trashes.GetChildren().OfType<TrashCompendiumEntry>())
            {
                trashEntry.Visible = true;
            }
            return;
        }

        string entryKey = ((Biomes)biomeIndex).ToString();
        Biome biome = GD.Load<Biome>($"{BiomesFolder}{entryKey}/{entryKey}.tres");
        string[] fishes = biome.Fishes.Select(f => f.Fish.ToString()).ToArray();
        string[] trashes = biome.Trashes.Select(f => f.Trash.ToString()).ToArray();
        foreach (var fishEntry in Fishes.GetChildren().OfType<FishCompendiumEntry>())
        {
            fishEntry.Visible = fishes.Contains(fishEntry.EntryKey);
        }
        foreach (var trashEntry in Trashes.GetChildren().OfType<TrashCompendiumEntry>())
        {
            trashEntry.Visible = trashes.Contains(trashEntry.EntryKey);
        }
    }
}
