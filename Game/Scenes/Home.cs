using Godot;
using System;
using WaffleStock;

public partial class Home : CanvasLayer
{
    [Export]
    RandomTimer FishTimer;
    [Export]
    Label Message;
    [Export]
    Node GameContainer;

    [Export]
    FishableButton ContinueButton;
    [Export]
    Button TestButton;

    [Export]
    DynamicBackground Background;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (OS.IsDebugBuild()) TestButton.Show();

        if (GameManager.GameSave != null)
        {
            ContinueButton.Show();
            GameManager.Biome = GD.Load<Biome>(GameManager.GameSave?.BiomePath);
        }

        if (GameManager.Biome == null)
            GameManager.Biome = GameManager.StartingBiome;

        Background._Ready();

        FishTimer.Start(GameManager.Biome.TimeToSpawnFish, GameManager.Biome.TimeToSpawnFishDeviation);
        if (GameManager.PrevScene == "res://Game/Scenes/Game/Game.tscn")
        {
            Message.Text = "Last Score:\n" + GameManager.Score.ToString();
        }
    }

    void Play(Game.Mode mode, Biome biome)
    {
        if (GameManager.GameSave != null)
        {
            GameManager.Mode = (Game.Mode)GameManager.GameSave?.Mode;
            uint playtime = (uint)Math.Ceiling(((TimeSpan)GameManager.GameSave?.TimePlayed).TotalSeconds);
            UserData.SetHighStat(Constants.LongestSession, playtime);
            UserData.IncrementStatistic(Constants.TotalTimePlayed, playtime);
            UserData.SetHighStat(Constants.HighScore, (long)GameManager.GameSave?.Score);
            SaveManager.SaveData();
            AchievementsManager.OnGameEnd();
            SaveManager.EraseGame();
        }
        else
        {
            UserData.IncrementStatistic(Constants.TotalGamesPlayed);
            AchievementsManager.OnGameStart();
        }

        GameManager.Mode = mode;
        GameManager.Biome = biome;
        GameManager.Score = 0;
        GameManager.Lives = 3;
        GameManager.StartTime = DateTime.Now;

        GameManager.ChangeSceneToFile("res://Game/Scenes/Game/Game.tscn");
    }

    void Continue()
    {
        GameManager.Mode = (Game.Mode)GameManager.GameSave?.Mode;
        GameManager.Biome = GD.Load<Biome>(GameManager.GameSave?.BiomePath);
        GameManager.Score = (long)GameManager.GameSave?.Score;
        GameManager.Lives = (uint)GameManager.GameSave?.Lives;
        GameManager.CurrentBiomeCatches = (int)GameManager.GameSave?.CurrentBiomeCatches;
        GameManager.CalculatedBiomeThreshold = (int)GameManager.GameSave?.CalculatedBiomeThreshold;
        GameManager.StartTime = (DateTime)(DateTime.Now - GameManager.GameSave?.TimePlayed);

        GameManager.ChangeSceneToFile("res://Game/Scenes/Game/Game.tscn");
    }

    void PlayClassic()
    {
        Play(Game.Mode.Classic, GameManager.StartingBiome);
    }

    void PlayTimeAttack()
    {
        Play(Game.Mode.TimeAttack, GameManager.StartingBiome);
    }

    void PlayGoGreen()
    {
        Play(Game.Mode.GoGreen, GameManager.StartingBiome);
    }

    void PlayTarget()
    {
        Play(Game.Mode.Target, GameManager.StartingBiome);
    }

    void PlayTest()
    {
        Play(Game.Mode.Classic, GameManager.TestBiome);
    }

    void GoToCompendium()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Compendium/Compendium.tscn");
    }

    void GoToStats()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Stats/Stats.tscn");
    }

    void GoToEquipment()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Equipment/Equipment.tscn");
    }

    void GoToSettings()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Settings/Settings.tscn");
    }

    void GoToTutorial()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Tutorial/Tutorial.tscn");
    }

    void GoToCredits()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Credits/Credits.tscn");
    }
}
