using Godot;
using System;
using WaffleStock;
using static SceneManager;

public partial class Home : CanvasLayer
{
    [Export]
    Label Message;

    [Export]
    FishableButton ContinueButton;
    [Export]
    Button TestButton;

    public override void _EnterTree()
    {
        base._EnterTree();

        if (GameManager.GameSave != null)
            GameManager.Biome = GD.Load<Biome>(GameManager.GameSave?.BiomePath);

        if (GameManager.Biome == null)
            GameManager.Biome = GameManager.StartingBiome;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (OS.IsDebugBuild()) TestButton.Show();

        if (GameManager.GameSave != null)
            ContinueButton.Show();

        if (SceneManager.PrevScene == "uid://0sb3i5bm7j4p")
            Message.Text = Tr("LAST_SCORE").Replace("{score}", GameManager.Score.ToString());

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

        UserData.IncrementStatistic(Constants.TotalGamesPlayed);
        AchievementsManager.OnGameStart();

        GameManager.Mode = mode;
        GameManager.Biome = biome;
        GameManager.Score = 0;
        GameManager.Lives = 3;
        GameManager.StartTime = DateTime.Now;

        ChangeSceneToFile(GameUID);
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

        ChangeSceneToFile(GameUID);
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
        GameManager.Mode = Game.Mode.Training;
        GameManager.Biome = GameManager.TestBiome;
        GameManager.Score = 0;
        GameManager.Lives = 3;
        GameManager.StartTime = DateTime.Now;

        ChangeSceneToFile(TrainingUID);
    }

    void GoToDebug()
    {
        ChangeSceneToFile(BiomeDebugUID);
    }

    void GoToCompendium()
    {
        ChangeSceneToFile(CompendiumUID);
    }

    void GoToStats()
    {
        ChangeSceneToFile(StatsUID);
    }

    void GoToEquipment()
    {
        ChangeSceneToFile(EquipmentUID);
    }

    void GoToSettings()
    {
        ChangeSceneToFile(SettingsUID);
    }

    void GoToTutorial()
    {
        ChangeSceneToFile(TutorialUID);
    }

    void GoToCredits()
    {
        ChangeSceneToFile(CreditsUID);
    }
}
