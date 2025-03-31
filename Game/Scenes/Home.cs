using Godot;
using Godot.Fish_the_fishes.Scripts;

public partial class Home : CanvasLayer
{
    [Export]
    private RandomTimer FishTimer;
    [Export]
    private Label Message;
    [Export]
    private Node GameContainer;
    [Export]
    private TextureRect Background;

    [Export]
    private Button TestButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (OS.IsDebugBuild()) TestButton.Show();
        if (GameManager.Biome == null) GameManager.Biome = GameManager.StartingBiome;
        FishTimer.Start(GameManager.Biome.TimeToSpawnFish, GameManager.Biome.TimeToSpawnFishDeviation);
        if (GameManager.PrevScene == "")
        {
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        }
        else
        {
            Message.Text = "Last Score:\n" + GameManager.Score.ToString();
        }
        Background.Texture = GameManager.Biome.Background;
    }

    private void Play(Game.Mode mode, Biome biome)
    {
        GameManager.Mode = mode;
        GameManager.Biome = biome;

        UserData.IncrementStatistic(Constants.TotalGamesPlayed);

        AchievementsManager.OnGameStart();

        GameManager.ChangeSceneToFile("res://Game/Scenes/Game/Game.tscn");
    }

    private void PlayClassic()
    {
        Play(Game.Mode.Classic, GameManager.StartingBiome);
    }

    private void PlayTimeAttack()
    {
        Play(Game.Mode.TimeAttack, GameManager.StartingBiome);
    }

    private void PlayGoGreen()
    {
        Play(Game.Mode.GoGreen, GameManager.StartingBiome);
    }

    private void PlayTarget()
    {
        Play(Game.Mode.Target, GameManager.StartingBiome);
    }

    private void PlayTest()
    {
        Play(Game.Mode.Classic, GameManager.TestBiome);
    }

    private void GoToCompendium()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Compendium/Compendium.tscn");
    }

    private void GoToStats()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Stats/Stats.tscn");
    }

    private void GoToEquipment()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Equipment/Equipment.tscn");
    }

    private void GoToSettings()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Settings/Settings.tscn");
    }

    private void GoToTutorial()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Tutorial/Tutorial.tscn");
    }

    private void GoToCredits()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Credits/Credits.tscn");
    }
    private void SpawnFish()
    {
        PackedScene FishScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Fishes));
        Fish fish = FishScene.Instantiate<Fish>();

        GameContainer.AddChild(fish);
    }
}
