using Godot;
using Wafflestock;
using System;
using System.Collections.Generic;

public partial class CompendiumEntry : PanelContainer
{

    [Export]
    protected Label CompendiumName;
    [Export]
    protected Label CompendiumDescription;
    [Export]
    protected Label NumberSeen;

    public Compendium.EntryType EntryType;

    public string EntryKey;
    protected string EntryFolder;

    protected UserData.CompendiumEntry Entry;
    protected IDescriptible Instance;

    public override void _Ready()
    {
        if (string.IsNullOrEmpty(EntryKey)) return;

        if (EntryType == Compendium.EntryType.Fish)
        {
            EntryFolder = $"{Constants.FishesFolder}{EntryKey}/";
            Instance = GD.Load<PackedScene>($"{EntryFolder}{EntryKey}.tscn").Instantiate<Fish>();
            Entry = UserData.FishCompendium.GetValueOrDefault(EntryKey);
        }
        else if (EntryType == Compendium.EntryType.Trash)
        {
            EntryFolder = $"{Constants.TrashesFolder}{EntryKey}/";
            Instance = GD.Load<PackedScene>($"{EntryFolder}{EntryKey}.tscn").Instantiate<Trash>();
            Entry = UserData.TrashCompendium.GetValueOrDefault(EntryKey);
        }
        else if (EntryType == Compendium.EntryType.Biome)
        {
            EntryFolder = $"{Constants.BiomesFolder}{EntryKey}/";
            Instance = GD.Load<Biome>($"{EntryFolder}{EntryKey}.tres");
            Entry = UserData.BiomeCompendium.GetValueOrDefault(EntryKey);
        }

        if (Entry == null) return;

        CompendiumName.Text = Instance.CompendiumName;
        NumberSeen.Text = Entry.Seen.ToString();
    }
}
