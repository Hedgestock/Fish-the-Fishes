using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;


public partial class Game : Node
{
    [Export]
    private TextureRect Background;
    [Export]
    private RandomTimer FishTimer;
    [Export]
    private RandomTimer TrashTimer;

    private DateTime StartTime;

    public enum Mode
    {
        AllModes,
        Menu,
        Classic,
        GoGreen,
        Target,
        Training,
        TimeAttack,
        Zen,
    }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GameManager.Score = 0;
        GameManager.Lives = 3;

        SetupBiome();
        GameManager.Instance.BiomeChanged += SetupBiome;
        StartTime = DateTime.Now;
    }

    public void EndGame()
    {
        uint playtime = (uint)Math.Ceiling((DateTime.Now - StartTime).TotalSeconds);
        UserData.SetHighStat(Constants.LongestSession, playtime);
        UserData.IncrementStatistic(Constants.TotalTimePlayed, playtime);
        UserData.SetHighStat(Constants.HighScore, GameManager.Score);
        GameManager.SaveData();
        AchievementsManager.OnGameEnd();
        GameManager.Mode = Mode.Menu;
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }

    private void SpawnFish()
    {
        PackedScene FishScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Fishes));
        Fish fish = FishScene.Instantiate<Fish>();

        // Spawn the fish by adding it to the main scene.
        AddChild(fish);
    }

    private void SpawnTrash()
    {
        PackedScene TrashScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Trashes));
        Trash trash = TrashScene.Instantiate<Trash>();
        Vector2 trashSpawnLocation = new Vector2(GD.Randi() % GameManager.ScreenSize.X, -100);
        trash.Position = trashSpawnLocation;
        trash.Velocity = new Vector2(GD.RandRange(-200, 200), 0);

        // Spawn the trash by adding it to the Main scene.
        AddChild(trash);
    }

    private void SetupBiome()
    {
        Background.Texture = GameManager.Biome.Background;
        GD.Print($"Biome threshold = {GameManager.CalculatedBiomeThreshold}");
        FishTimer.Start(GameManager.Biome.TimeToSpawnFish, GameManager.Biome.TimeToSpawnFishDeviation);
        TrashTimer.Start(GameManager.Biome.TimeToSpawnTrash, GameManager.Biome.TimeToSpawnTrashDeviation);
    }
}
