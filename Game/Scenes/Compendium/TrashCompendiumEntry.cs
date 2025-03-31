using Godot;
using Wafflestock;
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

        NumberHit.Text = (Entry as UserData.TrashCompendiumEntry).Hit.ToString();
        NumberCleaned.Text = (Entry as UserData.TrashCompendiumEntry).Cleaned.ToString();
        CompendiumDescription.Text = Instance.CompendiumDescription;

        ShowAnimationButtons();
    }
}
