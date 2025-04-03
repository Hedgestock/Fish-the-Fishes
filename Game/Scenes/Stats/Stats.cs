using Godot;
using WaffleStock;
using static WaffleStock.UserData.StatCategory;
using System;

public partial class Stats : CanvasLayer
{
    [Export]
    OptionButton Category;
    [Export]
    OptionButton Mode;
    [Export]
    StatLine HighScore;
    [Export]
    StatLine LongestSession;
    [Export]
    StatLine TotalTimePlayed;

    [ExportGroup("Classic")]
    [Export]
    StatLine TotalFishedFishes;
    [Export]
    StatLine TotalPointsScored;
    [Export]
    StatLine TotalTrashesHit;
    [Export]
    StatLine TotalLostFishes;
    [Export]
    StatLine MaxFishedFishes;
    [Export]
    StatLine MaxPointScored;

    [ExportGroup("Go Green")]
    [Export]
    StatLine TotalTrashesCleaned;
    [Export]
    StatLine TotalEatenFishes;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Adding those by code prevents mismatching IDs in case of human error or change of the Mode enum
        Mode.AddItem("All Modes", (int)Game.Mode.AllModes);
        Mode.AddItem("Classic", (int)Game.Mode.Classic);
        Mode.AddItem("Time Attack", (int)Game.Mode.TimeAttack);
        Mode.AddItem("Target", (int)Game.Mode.Target);
        Mode.AddItem("GoGreen", (int)Game.Mode.GoGreen);

        DisplayStats(0);
    }

    private void DisplayStats(int index)
    {
        UserData.StatCategory category = (UserData.StatCategory)Category.GetSelectedId();
        Game.Mode mode = (Game.Mode)Mode.GetSelectedId();

        HighScore.Score = UserData.GetStatistic(category, mode, Constants.HighScore);
        LongestSession.ScoreLabel.Text = SecondsToTime(UserData.GetStatistic(category, mode, Constants.LongestSession));
        TotalTimePlayed.ScoreLabel.Text = SecondsToTime(UserData.GetStatistic(category, mode, Constants.TotalTimePlayed));

        if (mode == Game.Mode.GoGreen)
        {
            TotalTrashesCleaned.Score = UserData.GetStatistic(Scratch, Game.Mode.GoGreen, Constants.TotalTrashesCleaned);
            TotalEatenFishes.Score = UserData.GetStatistic(Scratch, Game.Mode.GoGreen, Constants.TotalEatenFishes);
        }
        else
        {
            TotalFishedFishes.Score = UserData.GetStatistic(category, mode, Constants.TotalFishedFishes);
            TotalPointsScored.Score = UserData.GetStatistic(category, mode, Constants.TotalPointsScored);
            TotalTrashesHit.Score = UserData.GetStatistic(category, mode, Constants.TotalTrashesHit);
            TotalLostFishes.Score = UserData.GetStatistic(category, mode, Constants.TotalLostFishes);
            MaxFishedFishes.Score = UserData.GetStatistic(category, mode, Constants.MaxFishedFishes);
            MaxPointScored.Score = UserData.GetStatistic(category, mode, Constants.MaxPointScored);
        }

        TotalFishedFishes.Visible = mode != Game.Mode.GoGreen;
        TotalPointsScored.Visible = mode != Game.Mode.GoGreen;
        TotalTrashesHit.Visible = mode != Game.Mode.GoGreen;
        TotalLostFishes.Visible = mode != Game.Mode.GoGreen;
        MaxFishedFishes.Visible = mode != Game.Mode.GoGreen;
        MaxPointScored.Visible = mode != Game.Mode.GoGreen;

        TotalTrashesCleaned.Visible = mode == Game.Mode.GoGreen;
        TotalEatenFishes.Visible = mode == Game.Mode.GoGreen;

    }

    private string SecondsToTime(long seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        return string.Format("{0}h {1:D2}m {2:D2}s",
                        (int)t.TotalHours,
                        t.Minutes,
                        t.Seconds);
    }
    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
    }
}
