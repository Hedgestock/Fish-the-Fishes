using Godot;
using Godot.Fish_the_fishes.Scripts;


public partial class Aquarium : Node
{
    [Export]
    private RandomTimer FishTimer;
    [Export]
    private RandomTimer TrashTimer;
    [Export]
    private TextureRect Background;

    [Export]
    private TextureButton BackButton;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Background.Texture = GameManager.Biome.Background;
        FishTimer.Start(GameManager.Biome.TimeToSpawnFish, GameManager.Biome.TimeToSpawnFishDeviation);
        TrashTimer.Start(GameManager.Biome.TimeToSpawnTrash, GameManager.Biome.TimeToSpawnTrashDeviation);
        BackButton.Pressed += GameManager.GoToPreviousScene;
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
}
