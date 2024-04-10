using Godot;
using System;

public partial class GameManager : Node
{
    public Game.Mode Mode = Game.Mode.Classic;
    public uint Score = 0;
    public uint ClassicHighScore = 0;
    public uint TimeAttackHighScore = 0;
    public uint Lives = 3;
    public Vector2 ScreenSize;

    public string PrevScene = "";
    private string SaveFile = "user://data.save";
    private string SettingsFile = "user://settings.save";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LoadSave();
        ScreenSize = GetViewport().GetVisibleRect().Size;
        GD.Print(ScreenSize);
        GetTree().Root.SizeChanged += OnScreenResize;
    }

    private Godot.Collections.Dictionary<string, Variant> Save()
    {
        if (Mode == Game.Mode.Classic && Score > ClassicHighScore) ClassicHighScore = Score;
        if (Mode == Game.Mode.TimeAttack && Score > TimeAttackHighScore) TimeAttackHighScore = Score;
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            { "ClassicHighScore", ClassicHighScore },
            { "TimeAttackHighScore", TimeAttackHighScore },
        };
    }

    public void SaveGame()
    {
        using var gameSave = FileAccess.Open(SaveFile, FileAccess.ModeFlags.Write);

        gameSave.StoreLine(Json.Stringify(Save()));
    }

    private void LoadSave()
    {

        if (!FileAccess.FileExists(SaveFile))
        {
            return;
        }

        using var saveGame = FileAccess.Open(SaveFile, FileAccess.ModeFlags.Read);

        while (saveGame.GetPosition() < saveGame.GetLength())
        {
            var jsonString = saveGame.GetLine();

            // Creates the helper class to interact with JSON
            var json = new Json();
            var parseResult = json.Parse(jsonString);
            if (parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                continue;
            }

            // Get the data from the JSON object
            var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)json.Data);
            ClassicHighScore = (uint)nodeData["ClassicHighScore"];
            TimeAttackHighScore = (uint)nodeData["TimeAttackHighScore"];
        }
    }

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
