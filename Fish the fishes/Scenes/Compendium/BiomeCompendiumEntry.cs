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
        NumberSeen.Text = UserData.TrashCompendium[TypeString].Seen.ToString();

        if (UserData.BiomeCompendium[TypeString].Caught > 0)
        {
            CompendiumDescription.Text = (string)EntryType.GetProperty(nameof(CompendiumDescription)).GetValue(EntryType);
            if (Placeholder.SpriteFrames.GetAnimationNames().Length > 1)
            {
                AnimationButtons.Show();
            }
        }

    }
}
