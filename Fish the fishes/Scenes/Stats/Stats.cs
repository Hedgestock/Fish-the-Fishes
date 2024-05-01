using Godot;
using Godot.Fish_the_fishes.Scripts;
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
        CasualClassicHighScore.Text = TryGetValue(UserData.CasualScores, Game.Mode.Classic.ToString())?.ToString() ?? "0";
        CasualTimeAttackHighScore.Text = TryGetValue(UserData.CasualScores, Game.Mode.TimeAttack.ToString())?.ToString() ?? "0";
        CasualTargetHighScore.Text = TryGetValue(UserData.CasualScores, Game.Mode.Target.ToString())?.ToString() ?? "0";
        CasualGoGreenHighScore.Text = TryGetValue(UserData.CasualScores, Game.Mode.GoGreen.ToString())?.ToString() ?? "0";
        
        // Competitive High Scores
        CompetitiveClassicHighScore.Text = TryGetValue(UserData.CompetitiveScores, Game.Mode.Classic.ToString())?.ToString() ?? "0";
        CompetitiveTimeAttackHighScore.Text = TryGetValue(UserData.CompetitiveScores, Game.Mode.TimeAttack.ToString())?.ToString() ?? "0";
        CompetitiveTargetHighScore.Text = TryGetValue(UserData.CompetitiveScores, Game.Mode.Target.ToString())?.ToString() ?? "0";
        CompetitiveGoGreenHighScore.Text = TryGetValue(UserData.CompetitiveScores, Game.Mode.GoGreen.ToString())?.ToString() ?? "0";
        
        //Stats
        TotalFishedFishes.Text = TryGetValue(UserData.Statistics, Constants.TotalFishedFishes)?.ToString() ?? "0";
        TotalPointsScored.Text = TryGetValue(UserData.Statistics, Constants.TotalPointsScored)?.ToString() ?? "0";
        TotalTrashesHit.Text = TryGetValue(UserData.Statistics, Constants.TotalTrashesHit)?.ToString() ?? "0";
        TotalLostFishes.Text = TryGetValue(UserData.Statistics, Constants.TotalLostFishes)?.ToString() ?? "0";
        MaxFishedFishes.Text = TryGetValue(UserData.Statistics, Constants.MaxFishedFishes)?.ToString() ?? "0";
        MaxPointScored.Text = TryGetValue(UserData.Statistics, Constants.MaxPointScored)?.ToString() ?? "0";

        // Go Green Stats
        TotalTrashesCleaned.Text = TryGetValue(UserData.Statistics, Constants.TotalTrashesCleaned)?.ToString() ?? "0";
        TotalEatenFishes.Text = TryGetValue(UserData.Statistics, Constants.TotalEatenFishes)?.ToString() ?? "0";
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
            return tmp.ToString();
        }
        return null;
    }
}
