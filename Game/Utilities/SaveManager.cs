using Godot;
using Godot.FishTheFishes;
using System;

public partial class SaveManager : Node
{
    const string SaveDirectory = "user://";
    const string SaveFileName = "data.json";
    const string SaveFilePath = SaveDirectory + SaveFileName;
    const string SettingsFileName = "settings.cfg";
    const string SettingsFilePath = SaveDirectory + SettingsFileName;

    public override void _Ready()
    {
        base._Ready();
        UserSettings.Load(SettingsFilePath);
        LoadData();
    }

    static public void SaveData()
    {
        using var gameSave = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Write);

        gameSave.StoreString(UserData.Serialize());
    }

    static private void LoadData()
    {
        if (!FileAccess.FileExists(SaveFilePath))
        {
            GD.PrintErr("No save file to load");
            new UserData();
            return;
        }

        using var saveGame = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Read);

        string jsonString = saveGame.GetAsText();

        if (!UserData.Deserialize(jsonString))
        {
            GD.PrintErr("Failed to deserialize save file");
            new UserData();
        }
    }

    static public void EraseData()
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

    static public void SaveSettings()
    {
        UserSettings.Save(SettingsFilePath);
    }
}
