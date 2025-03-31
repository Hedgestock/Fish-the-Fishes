using Godot;
using Godot.FishTheFishes;
using System;

public partial class FishCompendiumEntry : AnimatedCompendiumEntry
{

    [Export]
    private Label NumberFished;
    [Export]
    private Label MaxSize;
    [Export]
    private Label MinSize;

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

        if (UserData.FishCompendium[EntryKey].MaxSize > 0)
        {
            MaxSize.Text = $"{UserData.FishCompendium[EntryKey].MaxSize:0.00}cm";
            MaxSize.Show();
            if (UserData.FishCompendium[EntryKey].MaxSize != UserData.FishCompendium[EntryKey].MinSize)
            {
                MaxSize.AddThemeColorOverride("font_color", new Color(0.12f, 0.6f, 0));
                MinSize.Text = $"{UserData.FishCompendium[EntryKey].MinSize:0.00}cm";
                MinSize.Show();
            }
        }
    }
}
