using Godot;
using Godot.Fish_the_fishes.Scripts;
using static Godot.Fish_the_fishes.Scripts.UserData.StatCategory;
using System;
using System.Collections.Generic;

public partial class Stats : CanvasLayer
{
    [Export]
    OptionButton Category;
    [Export]
    OptionButton Mode;
    [Export]
    StatLine HighScore;

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

    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }
}
