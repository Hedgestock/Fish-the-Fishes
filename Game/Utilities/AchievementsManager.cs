using Godot;
using System;
using WaffleStock;
using System.Collections.Generic;
using System.Linq;

public partial class AchievementsManager : CanvasLayer
{
    private PackedScene AchievementsNotificationScene = GD.Load<PackedScene>("uid://b4hl7jdtefkae");

    static private AchievementsManager _instance = null;
    public static AchievementsManager Instance { get { return _instance; } }

    private string AchievementsBasePath = "res://Game/Assets/Achievements/";
    public List<Achievement> AchievementsList = new();

    private Queue<PanelContainer> _achievementsQueue = new Queue<PanelContainer>();

    private AchievementsManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    public override void _Ready()
    {
        base._Ready();
        Layer = 10;
        foreach (string path in GetAchievements(AchievementsBasePath))
        {
            AchievementsList.Add(GD.Load<Achievement>(path));
        }
    }

    private List<string> GetAchievements(string dirPath)
    {
        List<string> achievementsPaths = new();

        foreach (string fileName in ResourceLoader.ListDirectory(dirPath))
        {
                if (fileName.EndsWith("/"))
                    achievementsPaths = achievementsPaths.Concat(GetAchievements(dirPath + fileName)).ToList();
                else if (fileName.GetExtension() == "tres")
                    achievementsPaths.Add(dirPath + fileName);
        }

        return achievementsPaths;
    }

    public static void CheckAll()
    {
        OnGameStart();
        OnGameEnd();
        OnFishFished();
        //OnPointsScored();
        //OnHit();

        // That is for unlocking potentially new equipment where the requirements are already met.
        foreach (var entry in UserData.Achievements)
        {
            Achievement achievement = GD.Load<Achievement>(entry.Key);

            // Here we remove outdated achievements
            if (achievement == null)
            {
                UserData.Achievements.Remove(entry.Key);
            }
            else if (!string.IsNullOrEmpty(achievement.UnlockableName) && !UserData.Equipments.ContainsKey(achievement.UnlockableName))
            {
                UserData.Equipments[achievement.UnlockableName] = new UserData.EquipmentStatus(achievement.UnlockableType);
            }
        }
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

                if (!string.IsNullOrEmpty(achievement.UnlockableName) && !UserData.Equipments.ContainsKey(achievement.UnlockableName))
                {
                    UserData.Equipments[achievement.UnlockableName] = new UserData.EquipmentStatus(achievement.UnlockableType);
                }

                PanelContainer AchievementNotification = _instance.AchievementsNotificationScene.Instantiate<PanelContainer>();

                AchievementNotification.GetNode<Label>("MarginContainer/HBoxContainer/VBoxContainer/AchievementName").Text = achievement.CompendiumName;

                // We have to display something if the queue is not yet active
                if (_instance._achievementsQueue.Count == 0)
                    _instance.AddChild(AchievementNotification);
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
