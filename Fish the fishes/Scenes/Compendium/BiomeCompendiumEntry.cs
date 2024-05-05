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
        if (string.IsNullOrEmpty(TypeString)) return;

        NumberSeen.Text = UserData.TrashCompendium[TypeString].Seen.ToString();

        if (UserData.BiomeCompendium[TypeString].Seen > 0)
        {

        }

    }
}
