using Godot;
using System;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public partial class AchievementsManager : Node
{

    [Export]
    public Array<Achievement> AchievementsList;

    [Export]
    private PackedScene AchievementsNotificationScene;

    static private AchievementsManager _instance = null;
    public static AchievementsManager Instance { get { return _instance; } }

    private Queue<PanelContainer> _achievementsQueue = new Queue<PanelContainer>();

    private AchievementsManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public static void CheckAll()
    {
        OnGameStart();
        OnGameEnd();
        OnFishFished();
        //OnPointsScored();
        //OnHit();
    }

    public static void OnGameStart()
    {
        CheckPredicate(IAchievable.CheckTiming.OnGameStart);
    }

    public static void OnGameEnd()
    {
        CheckPredicate(IAchievable.CheckTiming.OnGameEnd);
    }

    public static void OnFishFished()
    {
        CheckPredicate(IAchievable.CheckTiming.OnFishFished);
    }

    public static void OnPointsScored(int score)
    {
        CheckPredicate(IAchievable.CheckTiming.OnPointScored);
    }

    public static void OnHit(FishingLine.DamageType damageType)
    {
        CheckPredicate(IAchievable.CheckTiming.OnHit);
    }

    private static void CheckPredicate(IAchievable.CheckTiming timing)
    {
        List<Achievement> checkableAchievements = _instance.AchievementsList.Where((a) => a.Timing == timing).ToList();
        foreach (var achievement in checkableAchievements)
        {
            if (!UserData.Achievements.ContainsKey(achievement.ResourcePath) && achievement.Predicate())
            {
                UserData.Achievements.Add(achievement.ResourcePath, DateTime.Now);
                PanelContainer AchievementNotification = _instance.AchievementsNotificationScene.Instantiate<PanelContainer>();

                AchievementNotification.GetNode<Label>("MarginContainer/HBoxContainer/VBoxContainer/AchievementName").Text = achievement.CompendiumName;

                // We have to display something if the queue is not yet active
                if (_instance._achievementsQueue.Count == 0)
                    _instance.GetTree().Root.AddChild(AchievementNotification);
                _instance._achievementsQueue.Enqueue(AchievementNotification);
            }
        }
    }

    public static void DisplayNext()
    {
        // We let the currently displayed achievement in until it fades to signal to other achievements that something is still happening
        _instance._achievementsQueue.Dequeue();

        PanelContainer NextAchievementNotification;
        if (!_instance._achievementsQueue.TryPeek(out NextAchievementNotification)) return;

        _instance.GetTree().Root.AddChild(NextAchievementNotification);
    }

    public static void UnlockEquipment(string equipmentName, EquipmentPiece.Type type)
    {
        UserData.Equipments[equipmentName] = new UserData.EquipmentStatus(type);
    }
}
