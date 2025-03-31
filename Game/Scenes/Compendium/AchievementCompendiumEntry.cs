using Godot;
using Godot.FishTheFishes;
using System.Collections.Generic;

public partial class AchievementCompendiumEntry : CompendiumEntry
{

    [Export]
    private TextureRect Icon;


    [Export]
    private TextureProgressBar ProgressBar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        NumberSeen.Text = "???";

        Instance = GD.Load<Achievement>(EntryKey);

        if (Instance is DoneXTimes)
        {
            DoneXTimes tmpInstance = (DoneXTimes)Instance;
            ProgressBar.Show();
            ProgressBar.CustomMinimumSize = new Vector2(20, 20);
            ProgressBar.MaxValue = (Instance as DoneXTimes).Threshold;
            ProgressBar.Value = UserData.GetStatistic(tmpInstance.Category, tmpInstance.Mode, tmpInstance.Stat);
        }

        CompendiumName.Text = Instance.CompendiumName;
        if (!UserData.Achievements.ContainsKey(EntryKey)) return;

        //Icon.Texture = (Instance as Achievement).Background;
        CompendiumDescription.Text = Instance.CompendiumDescription;

        NumberSeen.Text = UserData.Achievements.GetValueOrDefault(EntryKey).ToString();
    }
}