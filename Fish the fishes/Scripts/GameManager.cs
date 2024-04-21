using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
    public Game.Mode Mode = Game.Mode.Classic;
    public uint Score = 0;
    public uint Lives = 3;
    public string PrevScene = "";
    public Vector2 ScreenSize;

    private static string SaveDirectory = "user://";
    private static string SaveFileName = "data.save";
    private static string SaveFilePath = SaveDirectory + SaveFileName;
    private static string SettingsFile = "settings.save";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LoadData();
        ScreenSize = GetViewport().GetVisibleRect().Size;
        GetTree().Root.SizeChanged += OnScreenResize;
    }

    public void WriteHighScore()
    {
        Dictionary<string, uint> scores = UserSettings.CompetitiveMode ? UserData.Instance.CompetitiveScores : UserData.Instance.CasualScores;
        if (!scores.ContainsKey(Mode.ToString()) || Score > scores[Mode.ToString()])
        {
            if (UserSettings.CompetitiveMode)
            {
                UserData.Instance.CompetitiveScores[Mode.ToString()] = Score;
            }
            else
            {
                UserData.Instance.CasualScores[Mode.ToString()] = Score;
            }
        }
    }

    public void SaveData()
    {
        using var gameSave = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Write);

        gameSave.StoreString(UserData.Serialize());
    }

    private void LoadData()
    {

        if (!FileAccess.FileExists(SaveFilePath))
        {
            GD.PrintErr("No save file to load");
            return;
        }

        using var saveGame = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Read);

        string jsonString = saveGame.GetAsText();

        if (!UserData.Deserialize(jsonString))
            GD.PrintErr("Failed to deserialize save file");
    }

    public void EraseData()
    {

        UserData.Reset();

        if (!FileAccess.FileExists(SaveFilePath))
        {
            GD.PrintErr("No save file to erase");
            return;
        }

        DirAccess dir = DirAccess.Open(SaveDirectory);

        dir.Remove(SaveFileName);
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
