using Godot;
using WaffleStock;
using System;


public partial class Test : Node
{
    [Export]
    TextureRect Background;
    [Export]
    RandomTimer FishTimer;
    [Export]
    RandomTimer TrashTimer;
    [Export]
    Container FishButtons;
    [Export]
    PackedScene FishButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GameManager.Score = 0;
        GameManager.Lives = 3;

        SetupBiome();
        GameManager.Instance.Connect(GameManager.SignalName.BiomeChanged, new Callable(this, MethodName.SetupBiome));
        foreach (var item in Enum.GetNames(typeof(Constants.Fishes)))
        {
            Button fb = FishButton.Instantiate<Button>();
            FishButtons.AddChild(fb);
        }

    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseButton iemb)
        {

        } else if (@event is InputEventKey iek)
        {
            if (iek.Pressed && iek.Keycode == Key.Space)
            {
                GetTree().Paused = !GetTree().Paused;
            }
        }
    }

    //void PlaceFish(Vector2 position)
    //{
    //    Fish fish = GD.Load<PackedScene>().Instantiate<Fish>();
    //    fish.Position = position;
    //    AddChild(fish);
    //}

    void SpawnFish()
    {
        PackedScene FishScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Fishes));
        Fish fish = FishScene.Instantiate<Fish>();

        // Spawn the fish by adding it to the main scene.
        AddChild(fish);
    }

    void SpawnTrash()
    {
        PackedScene TrashScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Trashes));
        Trash trash = TrashScene.Instantiate<Trash>();
        Vector2 trashSpawnLocation = new Vector2(GD.Randi() % GameManager.ScreenSize.X, -100);
        trash.Position = trashSpawnLocation;
        trash.Velocity = new Vector2(GD.RandRange(-200, 200), 0);

        // Spawn the trash by adding it to the Main scene.
        AddChild(trash);
    }

    void SetupBiome()
    {
        Background.Texture = GameManager.Biome.Background;
        AudioManager.PlayMusic(GameManager.Biome.Music);
        //FishTimer.Start(GameManager.Biome.TimeToSpawnFish, GameManager.Biome.TimeToSpawnFishDeviation);
        //TrashTimer.Start(GameManager.Biome.TimeToSpawnTrash, GameManager.Biome.TimeToSpawnTrashDeviation);
    }
}
