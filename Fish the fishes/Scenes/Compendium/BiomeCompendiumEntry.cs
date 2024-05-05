using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class BiomeCompendiumEntry : CompendiumEntry
{

    [Export]
    private TextureRect Background;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (string.IsNullOrEmpty(EntryKey)) return;

        NumberSeen.Text = UserData.TrashCompendium[EntryKey].Seen.ToString();

        if (UserData.BiomeCompendium[EntryKey].Seen > 0)
        {

        }

    }
}
