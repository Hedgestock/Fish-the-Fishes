using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

public partial class AchievementCompendiumEntry : CompendiumEntry
{

    [Export]
    private TextureRect Icon;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        NumberSeen.Text = "???";

        if (!UserData.Achievements.ContainsKey(EntryKey)) return;

        Instance = GD.Load<Achievement>(EntryKey);

        CompendiumName.Text = Instance.CompendiumName;
        //Icon.Texture = (Instance as Achievement).Background;
        CompendiumDescription.Text = Instance.CompendiumDescription;

        NumberSeen.Text = UserData.Achievements.GetValueOrDefault(EntryKey).ToString();
    }
}