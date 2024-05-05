using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class TrashCompendiumEntry : AnimatedCompendiumEntry
{

    [Export]
    private Label NumberCleaned;
    [Export]
    private Label NumberHit;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        if (Entry == null) return;

        NumberHit.Text = UserData.TrashCompendium[EntryKey].Hit.ToString();
        NumberCleaned.Text = UserData.TrashCompendium[EntryKey].Cleaned.ToString();
        CompendiumDescription.Text = Instance.CompendiumDescription;

        ShowAnimationButtons();
    }
}
