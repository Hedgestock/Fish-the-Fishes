using Godot;
using System;

public partial class GameManager : Node
{
    public Game.Mode mode = Game.Mode.Classic;
    public uint score = 0;
    public uint highScore = 0;
    public uint lives = 3;

    public string PrevScene = "";
    private string saveFile = "user://data.save";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LoadSave();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private Godot.Collections.Dictionary<string, Variant> Save()
    {
        if (score > highScore) { highScore = score; }
        return new Godot.Collections.Dictionary<string, Variant>()
        {
            { "HighScore", highScore },
        };
    }

    public void SaveGame()
    {
        using var gameSave = FileAccess.Open(saveFile, FileAccess.ModeFlags.Write);

        gameSave.StoreLine(Json.Stringify(Save()));
    }

    private void LoadSave()
    {

        if (!FileAccess.FileExists(saveFile))
        {
            return;
        }

        using var saveGame = FileAccess.Open(saveFile, FileAccess.ModeFlags.Read);

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
            highScore = (uint)nodeData["HighScore"];
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

    
}
