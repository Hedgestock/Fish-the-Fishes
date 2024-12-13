using Godot;
using System;

public partial class BiomeCompendiumEntry : CompendiumEntry
{

    [Export]
    private TextureRect Background;

    [Export]
    public TextureButton Icon;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Background.Texture = (Instance as Biome).Background;

        if (Entry == null) return;

        Icon.TextureNormal = (Instance as Biome).Background;
        Icon.TexturePressed = Icon.TextureNormal;
        Icon.TextureHover = Icon.TextureNormal;
        Icon.TextureDisabled = Icon.TextureNormal;
        CompendiumDescription.Text = Instance.CompendiumDescription;
    }
}
