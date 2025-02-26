using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node
{
    [Signal]
    public delegate void TargetChangedEventHandler();

    [Signal]
    public delegate void BiomeChangedEventHandler();

    [Export]
    private Biome _startingBiome;
    [Export]
    private Biome _testBiome;

    public static Biome StartingBiome { get { return _instance._startingBiome; } }
    public static Biome TestBiome { get { return _instance._testBiome; } }

    static private GameManager _instance = null;
    public static GameManager Instance { get { return _instance; } }

    private static string _target = "Fish";
    static public string Target { get { return _target; } }


    private static Biome _biome;
    public static Biome Biome
    {
        get { return _biome; }
        set
        {
            _biome = value;

            _currentBiomeCatches = 0;
            CalculatedBiomeThreshold = (int)GD.Randfn(value.ChangeBiomeThreshold, value.ChangeBiomeThresholdDeviation);

            for (int i = 0; i < 20; i++)
            {
                GD.Print((int)GD.Randfn(value.ChangeBiomeThreshold, value.ChangeBiomeThresholdDeviation));
            }

            Instance.EmitSignal(SignalName.BiomeChanged);

            if (Mode == Game.Mode.Menu) return;
            if (UserData.BiomeCompendium.TryGetValue(value.ResourceName, out UserData.BiomeCompendiumEntry entry)) entry.Seen++;
            else UserData.BiomeCompendium[Biome.ResourceName] = new UserData.BiomeCompendiumEntry();
        }
    }


    public static Game.Mode Mode = Game.Mode.Menu;
    public static long Score = 0;
    public static uint Lives = 3;
    private static int _currentBiomeCatches = 0;
    public static int CurrentBiomeCatches
    {
        get { return _currentBiomeCatches; }
        set
        {
            GD.Print(value);
            if (value > CalculatedBiomeThreshold)
            {
                ChangeBiome();
            }
            else
            {
                _currentBiomeCatches = value;
            }
        }
    }
    public static int CalculatedBiomeThreshold = 0;

    public static string PrevScene = "";
    public static Vector2 ScreenSize;

    private static string SaveDirectory = "user://";
    private static string SaveFileName = "data.save";
    private static string SaveFilePath = SaveDirectory + SaveFileName;
    private static string SettingsFileName = "settings.save";
    private static string SettingsFilePath = SaveDirectory + SettingsFileName;

    private static SceneTreeTimer TargetSafeTimer;

    private GameManager()
    {
        if (_instance != null)
            return;
        _instance = this;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LoadSettings();
        LoadData();
        ScreenSize = GetViewport().GetVisibleRect().Size;
        GetTree().Root.SizeChanged += OnScreenResize;
    }

    static public void ChangeTarget()
    {
        if (TargetSafeTimer != null)
        {
            TargetSafeTimer.Timeout -= ChangeTarget;
            TargetSafeTimer = null;
        }
        _target = (WeightedItem.ChooseFrom(Biome.Fishes.ToArray()) as WeightedFish).Fish.ToString();
        _instance.EmitSignal(SignalName.TargetChanged);
    }

    static public void ChangeBiome()
    {
        if (Biome.FollowupBiomes.Count == 0) return;
        Biome = GD.Load<Biome>(Biome.GetRandomPathFrom(Biome.FollowupBiomes));

        // That's a mouthfull, but we simply check the current biome to check if the target is still valid
        // otherwise, we just wait a bit to avoid the issue of fishing one already on screen and set a new one.
        if (Mode == Game.Mode.Target && !Biome.Fishes.Select(fish => fish.ToString()).Contains(Target))
        {
            TargetSafeTimer = _instance.GetTree().CreateTimer(10);
            TargetSafeTimer.Timeout += ChangeTarget;
        };
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
        using var settings = FileAccess.Open(SettingsFilePath, FileAccess.ModeFlags.Write);

        settings.StoreString(UserSettings.Serialize());
    }

    static private void LoadSettings()
    {
        if (!FileAccess.FileExists(SettingsFilePath))
        {
            GD.PrintErr("No settings file to load");
            new UserSettings();
            return;
        }

        using var settings = FileAccess.Open(SettingsFilePath, FileAccess.ModeFlags.Read);

        string jsonString = settings.GetAsText();

        if (!UserSettings.Deserialize(jsonString))
        {
            GD.PrintErr("Failed to deserialize settings file");
            new UserSettings();
        }
    }

    static public void ChangeSceneToFile(string file)
    {
        PrevScene = _instance.GetTree().CurrentScene.SceneFilePath;
        _instance.GetTree().ChangeSceneToFile(file);
    }

    static public void ChangeSceneToPacked(PackedScene scene)
    {
        PrevScene = _instance.GetTree().CurrentScene.SceneFilePath;
        _instance.GetTree().ChangeSceneToPacked(scene);
    }

    static public void GoToPreviousScene()
    {
        var _tmpPrevScene = _instance.GetTree().CurrentScene.SceneFilePath;
        _instance.GetTree().ChangeSceneToFile(PrevScene);
        PrevScene = _tmpPrevScene;
    }

    private void OnScreenResize()
    {
        ScreenSize = GetViewport().GetVisibleRect().Size;
    }

    private void Test()
    {
        List<string> list = new List<string>();
        TestRec(list, StartingBiome);
        list.ForEach(item => GD.Print("biome: ", item));
    }
    private void TestRec(List<string> list, Biome CurrentBiome)
    {
        if (list.Contains(CurrentBiome.ResourceName)) return;
        list.Add(CurrentBiome.ResourceName);
        foreach (var item in CurrentBiome.FollowupBiomes)
        {
            string BiomeType = item.Biome.ToString();
            TestRec(list, GD.Load<Biome>($"{Constants.BiomesFolder}{BiomeType}/{BiomeType}.tres"));
        }
    }
}
