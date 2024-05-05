using Godot;
using System;

public partial class BiomeCompendiumEntry : CompendiumEntry
{

    [Export]
    private TextureRect Background;

    [Export]
    private TextureRect Icon;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Background.Texture = (Instance as Biome).Background;

        if (Entry == null) return;

        Icon.Texture = (Instance as Biome).Background;
        CompendiumDescription.Text = Instance.CompendiumDescription;
    }
}
