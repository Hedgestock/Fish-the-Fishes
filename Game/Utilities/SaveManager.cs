using Godot;
using System;
using System.Text.Json;
using WaffleStock;

public partial class SaveManager : Node
{
    const string SaveDirectory = "user://";
    const string DataFileName = "data.json";
    const string DataFilePath = SaveDirectory + DataFileName;
    const string SettingsFileName = "settings.cfg";
    const string SettingsFilePath = SaveDirectory + SettingsFileName;
    const string GameFileName = "game.json";
    const string GameFilePath = SaveDirectory + GameFileName;

    public override void _Ready()
    {
        base._Ready();
        UserSettings.Load(SettingsFilePath);
        LoadData();
        LoadGame();
    }

    static public void SaveData()
    {
        using var gameSave = FileAccess.Open(DataFilePath, FileAccess.ModeFlags.Write);

        gameSave.StoreString(UserData.Serialize());
    }

    static private void LoadData()
    {
        if (!FileAccess.FileExists(DataFilePath))
        {
            GD.PrintErr("No data file to load");
            new UserData();
            return;
        }

        using var saveGame = FileAccess.Open(DataFilePath, FileAccess.ModeFlags.Read);

        string jsonString = saveGame.GetAsText();

        if (!UserData.Deserialize(jsonString))
        {
            GD.PrintErr("Failed to deserialize data file");
            new UserData();
        }
    }

    static public void EraseData()
    {

        UserData.Reset();

        if (!FileAccess.FileExists(DataFilePath))
        {
            GD.PrintErr("No data file to erase");
            return;
        }

        DirAccess dir = DirAccess.Open(SaveDirectory);

        dir.Remove(DataFileName);
    }

    static public void SaveSettings()
    {
        UserSettings.Save(SettingsFilePath);
    }

    static public void SaveGame()
    {
        SaveData();
        GameManager.GameSave = new()
        {
            Mode = GameManager.Mode,
            BiomePath = GameManager.Biome.ResourcePath,
            Score = GameManager.Score,
            Lives = GameManager.Lives,
            CurrentBiomeCatches = GameManager.CurrentBiomeCatches,
            CalculatedBiomeThreshold = GameManager.CalculatedBiomeThreshold,
            TimePlayed = DateTime.Now - GameManager.StartTime
        };

        using var gameSave = FileAccess.Open(GameFilePath, FileAccess.ModeFlags.Write);

        gameSave.StoreString(JsonSerializer.Serialize(GameManager.GameSave));
    }

    static public bool LoadGame()
    {
        if (!FileAccess.FileExists(GameFilePath))
        {
            GD.PrintErr("No game file to load");
            return false;
        }

        using var saveGame = FileAccess.Open(GameFilePath, FileAccess.ModeFlags.Read);

        string jsonString = saveGame.GetAsText();

        try
        {
            GameManager.GameSave = JsonSerializer.Deserialize<GameSave>(jsonString);
            return true;
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
            GD.PrintErr("Failed to deserialize game file");
            return false;
        }
    }

    static public void EraseGame()
    {
        GameManager.GameSave = null;

        if (!FileAccess.FileExists(GameFilePath))
        {
            GD.PrintErr("No game file to erase");
            return;
        }

        DirAccess dir = DirAccess.Open(SaveDirectory);

        dir.Remove(GameFilePath);
    }


    public struct GameSave
    {
        public Game.Mode Mode { get; set; }
        public string BiomePath { get; set; }
        public long Score { get; set; }
        public uint Lives { get; set; }
        public int CurrentBiomeCatches { get; set; }
        public int CalculatedBiomeThreshold { get; set; }
        public TimeSpan TimePlayed { get; set; }
    }
}
