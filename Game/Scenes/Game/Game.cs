using Godot;
using WaffleStock;
using System;


public partial class Game : Node
{
    [Export]
    private RandomTimer FishTimer;
    [Export]
    private RandomTimer TrashTimer;
    [Export]
    private TextureRect Background;
    private TextureRect BackgroundTransition;

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
        BackgroundTransition = Background.GetNode<TextureRect>("BackgroundTransition");
        BackgroundTransition.Texture = GameManager.Biome.Background;
        SetupBiome();
        GameManager.Instance.Connect(GameManager.SignalName.BiomeChanged, new Callable(this, MethodName.SetupBiome));
    }

    public void EndGame()
    {
        AudioManager.StopMusic();
        uint playtime = (uint)Math.Ceiling((DateTime.Now - GameManager.StartTime).TotalSeconds);
        UserData.SetHighStat(Constants.LongestSession, playtime);
        UserData.IncrementStatistic(Constants.TotalTimePlayed, playtime);
        UserData.SetHighStat(Constants.HighScore, GameManager.Score);
        SaveManager.SaveData();
        AchievementsManager.OnGameEnd();
        GameManager.Mode = Mode.Menu;
        SaveManager.EraseGame();
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
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
        if (Background.Texture != BackgroundTransition.Texture) {
            BackgroundTransition.Show();
            Tween backgroundTween = CreateTween();
            backgroundTween.TweenProperty(BackgroundTransition, "modulate:a", 0, .5f);
            backgroundTween.TweenCallback(Callable.From(() =>
                {
                    BackgroundTransition.Hide();
                    BackgroundTransition.Modulate = Colors.White;
                    BackgroundTransition.Texture = Background.Texture;
                }
            ));
        }
        AudioManager.PlayMusic(GameManager.Biome.Music);
        GD.Print($"Biome threshold = {GameManager.CalculatedBiomeThreshold}");
        FishTimer.Start(GameManager.Biome.TimeToSpawnFish, GameManager.Biome.TimeToSpawnFishDeviation);
        TrashTimer.Start(GameManager.Biome.TimeToSpawnTrash, GameManager.Biome.TimeToSpawnTrashDeviation);
        SaveManager.SaveGame();
    }
}
