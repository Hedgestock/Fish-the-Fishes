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
    Label CasualClassicHighScore;
    [Export]
    Label CasualTimeAttackHighScore;
    [Export]
    Label CompetitiveClassicHighScore;
    [Export]
    Label CompetitiveTimeAttackHighScore;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TotalFishedFishes.Text = TryGetValue(UserData.Instance.Statistics, Constants.TotalFishedFishes)?.ToString() ?? "0";
        TotalPointsScored.Text = TryGetValue(UserData.Instance.Statistics, Constants.TotalPointsScored)?.ToString() ?? "0";
        TotalTrashesHit.Text = TryGetValue(UserData.Instance.Statistics, Constants.TotalTrashesHit)?.ToString() ?? "0";
        TotalLostFishes.Text = TryGetValue(UserData.Instance.Statistics, Constants.TotalLostFishes)?.ToString() ?? "0";
        MaxFishedFishes.Text = TryGetValue(UserData.Instance.Statistics, Constants.MaxFishedFishes)?.ToString() ?? "0";
        MaxPointScored.Text = TryGetValue(UserData.Instance.Statistics, Constants.MaxPointScored)?.ToString() ?? "0";

        CasualClassicHighScore.Text = TryGetValue(UserData.Instance.CasualScores, Game.Mode.Classic.ToString())?.ToString() ?? "0";
        CasualTimeAttackHighScore.Text = TryGetValue(UserData.Instance.CasualScores, Game.Mode.TimeAttack.ToString())?.ToString() ?? "0";
        CompetitiveClassicHighScore.Text = TryGetValue(UserData.Instance.CompetitiveScores, Game.Mode.Classic.ToString())?.ToString() ?? "0";
        CompetitiveTimeAttackHighScore.Text = TryGetValue(UserData.Instance.CompetitiveScores, Game.Mode.TimeAttack.ToString())?.ToString() ?? "0";
    }


    private void GoToHome()
    {
        GetNode<GameManager>("/root/GameManager").ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }

    private string? TryGetValue<T>(Dictionary<string, T> dic, string key)
    {
        T tmp;
        if (dic.TryGetValue(key, out tmp))
        {
            return tmp.ToString();
        }
        return null;
    }
}
