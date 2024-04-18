using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
    public Game.Mode Mode = Game.Mode.Classic;
    public uint Score = 0;
    public uint Lives = 3;
    public Vector2 ScreenSize;

    public string PrevScene = "";
    private string SaveFile = "user://data.save";
    private string SettingsFile = "user://settings.save";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LoadData();
        ScreenSize = GetViewport().GetVisibleRect().Size;
        GD.Print(UserData.Serialize());
        GetTree().Root.SizeChanged += OnScreenResize;
    }

    public void WriteHighScore()
    {
        //Dictionary<string, uint> scores = UserSettings.CompetitiveMode ? UserData.CompetitiveScores : UserData.CasualScores;
        //if (Score > scores[Mode.ToString()])
        //{
        //    if (UserSettings.CompetitiveMode)
        //    {
        //        UserData.CompetitiveScores[Mode.ToString()] = Score;
        //    }
        //    else
        //    {
        //        UserData.CasualScores[Mode.ToString()] = Score;
        //    }
        //}
        GD.Print(UserData.Serialize());
    }

    public void SaveData()
    {
        using var gameSave = FileAccess.Open(SaveFile, FileAccess.ModeFlags.Write);

        gameSave.StoreString(UserData.Serialize());
    }

    private void LoadData()
    {

        if (!FileAccess.FileExists(SaveFile))
        {
            return;
        }

        using var saveGame = FileAccess.Open(SaveFile, FileAccess.ModeFlags.Read);

        string jsonString = saveGame.GetAsText();

        UserData.Deserialize(jsonString);
    }

    //private void LoadSettings()
    //{

    //    if (!FileAccess.FileExists(SettingsFile))
    //    {
    //        return;
    //    }

    //    using var saveGame = FileAccess.Open(SettingsFile, FileAccess.ModeFlags.Read);

    //    string jsonString = saveGame.GetAsText();

    //    UserSettings.Deserialize(jsonString);
    //}

    public void ChangeSceneToFile(string file)
    {
        PrevScene = GetTree().CurrentScene.SceneFilePath;
        GetTree().ChangeSceneToFile(file);
    }

    public void ChangeSceneToPacked(PackedScene scene)
    {
        PrevScene = GetTree().CurrentScene.SceneFilePath;
        GetTree().ChangeSceneToPacked(scene);
    }

    public void GoToPreviousScene()
    {
        var _tmpPrevScene = GetTree().CurrentScene.SceneFilePath;
        GetTree().ChangeSceneToFile(PrevScene);
        PrevScene = _tmpPrevScene;
    }

    private void OnScreenResize()
    {
        ScreenSize = GetViewport().GetVisibleRect().Size;
    }
}
