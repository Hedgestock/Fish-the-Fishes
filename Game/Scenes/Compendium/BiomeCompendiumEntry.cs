using Godot;

public partial class BiomeCompendiumEntry : CompendiumEntry
{

    [Export]
    private TextureRect Background;

    [Export]
    public TextureRect Icon;

    [Export]
    public TextureButton Button;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        Background.Texture = (Instance as Biome).Background;

        if (Entry == null) return;

        Icon.Texture = (Instance as Biome).Background;

        Button.TextureNormal = Icon.Texture;
        Button.TexturePressed = Button.TextureNormal;
        Button.TextureHover = Button.TextureNormal;
        Button.TextureDisabled = Button.TextureNormal;

        Button.Pressed += LaunchAquarium;

        CompendiumDescription.Text = Instance.CompendiumDescription;
    }

    private void LaunchAquarium()
    {
        GameManager.Biome = Instance as Biome;
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Aquarium/Aquarium.tscn");
    }
}
