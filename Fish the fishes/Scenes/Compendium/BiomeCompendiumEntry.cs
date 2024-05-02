using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class BiomeCompendiumEntry : CompendiumEntry
{

    [Export]
    private Label NumberSeen;
    [Export]
    private TextureRect Background;

    public Biome Biome;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        NumberSeen.Text = UserData.TrashCompendium[TypeString].Seen.ToString();
    }
}
