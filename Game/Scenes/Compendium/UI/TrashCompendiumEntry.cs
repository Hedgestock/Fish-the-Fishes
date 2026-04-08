using Godot;
using WaffleStock;
using System;

public partial class TrashCompendiumEntry : AnimatedCompendiumEntry
{

    [Export]
    private Label Hit;
    [Export]
    private Label Cleaned;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        if (Entry == null) return;

        Hit.Text = Tr("HIT").Replace("0", (Entry as UserData.TrashCompendiumEntry).Hit.ToString());
        Cleaned.Text = Tr("CLEANED").Replace("0", (Entry as UserData.TrashCompendiumEntry).Cleaned.ToString());
        CompendiumDescription.Text = Instance.CompendiumDescription;

        ShowAnimationButtons();
    }
}
