using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class FishCompendiumEntry : AnimatedCompendiumEntry
{

    [Export]
    private Label NumberFished;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        if (Entry == null) return;

        NumberFished.Text = (Entry as UserData.FishCompendiumEntry).Caught.ToString();

        if (UserData.FishCompendium[EntryKey].Caught > 0)
        {
            CompendiumDescription.Text = Instance.CompendiumDescription;
            ShowAnimationButtons();
        }
    }
}
