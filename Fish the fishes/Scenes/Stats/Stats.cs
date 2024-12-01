using Godot;
using Godot.Fish_the_fishes.Scripts;
using static Godot.Fish_the_fishes.Scripts.UserData.StatCategory;
using System;
using System.Collections.Generic;

public partial class Stats : CanvasLayer
{
    [Export]
    Label TotalFishedFishes;
    [Export]
    Label TotalPointsScored;
    [Export]
    Label TotalTrashesHit;
    [Export]
    Label TotalLostFishes;
    [Export]
    Label MaxFishedFishes;
    [Export]
    Label MaxPointScored;
    [Export]
    Label TotalTrashesCleaned;
    [Export]
    Label TotalEatenFishes;

    [ExportGroup("CasualHighScores")]
    [Export]
    Label CasualClassicHighScore;
    [Export]
    Label CasualTimeAttackHighScore;
    [Export]
    Label CasualTargetHighScore;
    [Export]
    Label CasualGoGreenHighScore;
    [ExportGroup("CompetitiveHighScores")]
    [Export]
    Label CompetitiveClassicHighScore;
    [Export]
    Label CompetitiveTimeAttackHighScore;
    [Export]
    Label CompetitiveTargetHighScore;
    [Export]
    Label CompetitiveGoGreenHighScore;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Casual High Scores
        CasualClassicHighScore.Text = UserData.GetStatistic(Casual, Game.Mode.Classic, Constants.HighScore).ToString() ?? "0";
        CasualTimeAttackHighScore.Text = UserData.GetStatistic(Casual, Game.Mode.TimeAttack, Constants.HighScore).ToString() ?? "0";
        CasualTargetHighScore.Text = UserData.GetStatistic(Casual, Game.Mode.Target, Constants.HighScore).ToString() ?? "0";
        CasualGoGreenHighScore.Text = UserData.GetStatistic(Casual, Game.Mode.GoGreen, Constants.HighScore).ToString() ?? "0";

        // Competitive High Scores
        CompetitiveClassicHighScore.Text = UserData.GetStatistic(Competitive, Game.Mode.Classic, Constants.HighScore).ToString() ?? "0";
        CompetitiveTimeAttackHighScore.Text = UserData.GetStatistic(Competitive, Game.Mode.Classic, Constants.HighScore).ToString() ?? "0";
        CompetitiveTargetHighScore.Text = UserData.GetStatistic(Competitive, Game.Mode.Classic, Constants.HighScore).ToString() ?? "0";
        CompetitiveGoGreenHighScore.Text = UserData.GetStatistic(Competitive, Game.Mode.Classic, Constants.HighScore).ToString() ?? "0";
        
        //Stats
        TotalFishedFishes.Text = UserData.GetStatistic(Scratch, Game.Mode.AllModes, Constants.TotalFishedFishes).ToString() ?? "0";
        TotalPointsScored.Text = UserData.GetStatistic(Scratch, Game.Mode.AllModes, Constants.TotalPointsScored).ToString() ?? "0";
        TotalTrashesHit.Text = UserData.GetStatistic(Scratch, Game.Mode.AllModes, Constants.TotalTrashesHit).ToString() ?? "0";
        TotalLostFishes.Text = UserData.GetStatistic(Scratch, Game.Mode.AllModes, Constants.TotalLostFishes).ToString() ?? "0";
        MaxFishedFishes.Text = UserData.GetStatistic(Scratch, Game.Mode.AllModes, Constants.MaxFishedFishes).ToString() ?? "0";
        MaxPointScored.Text = UserData.GetStatistic(Scratch, Game.Mode.AllModes, Constants.MaxPointScored).ToString() ?? "0";

        // Go Green Stats
        TotalTrashesCleaned.Text = UserData.GetStatistic(Scratch, Game.Mode.GoGreen, Constants.TotalTrashesCleaned).ToString() ?? "0";
        TotalEatenFishes.Text = UserData.GetStatistic(Scratch, Game.Mode.GoGreen, Constants.TotalEatenFishes).ToString() ?? "0";
    }


    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }

    private string TryGetValue<T>(Dictionary<string, T> dic, string key)
    {
        T tmp;
        if (dic.TryGetValue(key, out tmp))
        {
            return tmp?.ToString();
        }
        return null;
    }
}
