using Godot;
using System;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using System.Collections.Generic;
using System.Linq;

public partial class AchievementsManager : Node
{

    [Export]
    private Array<Achievement> AchievementsList;

    [Export]
    private PackedScene AchievementsNotificationScene;

    static private AchievementsManager _instance = null;
    public static AchievementsManager Instance { get { return _instance; } }

    private AchievementsManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public static void OnGameStart()
    {
        GD.Print("game started");
        CheckPredicate(IAchievable.CheckTiming.OnGameStart);
    }

    public static void OnGameEnd()
    {
        GD.Print("game ended");
        CheckPredicate(IAchievable.CheckTiming.OnGameEnd);
    }

    public static void OnFishFished()
    {
        GD.Print("fish fished");
        CheckPredicate(IAchievable.CheckTiming.OnFishFished);
    }

    public static void OnPointsScored(int score)
    {
        GD.Print("point scored");
        CheckPredicate(IAchievable.CheckTiming.OnPointScored);
    }

    public static void OnHit(FishingLine.DamageType damageType)
    {
        GD.Print("hit");
        CheckPredicate(IAchievable.CheckTiming.OnHit);
    }

    private static void CheckPredicate(IAchievable.CheckTiming timing)
    {
        List<Achievement> checkableAchievements = _instance.AchievementsList.Where((a) => a.Timing == timing).ToList();
        foreach (var achievement in checkableAchievements)
        {
            if (!UserData.Achievements.ContainsKey(achievement.ResourceName) && achievement.Predicate())
            {
                UserData.Achievements.Add(achievement.ResourceName, DateTime.Now);
                PanelContainer AchievementNotification = _instance.AchievementsNotificationScene.Instantiate<PanelContainer>();

                AchievementNotification.GetNode<Label>("MarginContainer/HBoxContainer/VBoxContainer/AchievementName").Text = achievement.CompendiumName;

                _instance.GetTree().Root.AddChild(AchievementNotification);
            }
        }
    }
}
