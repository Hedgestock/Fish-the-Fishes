using Godot;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using Godot.NativeInterop;
using System;
using System.Linq;

public partial class Game : Node
{
    [Export]
    private TextureRect Background;

    public enum Mode
    {
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

        Background.Texture = GameManager.Biome.Background;
    }

    public void EndGame()
    {
        GameManager.WriteHighScore();
        GameManager.SaveData();
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

    private void Despawn(Node2D body)
    {
        if (body is IFishable && (body as IFishable).IsCaught) return;
        body.QueueFree();
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

    private void ChangeBiome()
    {
        if (GameManager.Biome.FollowupBiomes.Count == 0) return;
        GameManager.Biome = GD.Load<Biome>(Biome.GetRandomPathFrom(GameManager.Biome.FollowupBiomes));
        Background.Texture = GameManager.Biome.Background;

        // That's a mouthfull, but we simply check the current biome to check if the target is still valid
        // otherwise, we just wait a bit to avoid the issue of fishing one already on screen and set a new one.
        if (GameManager.Mode == Mode.Target && !GameManager.Biome.Fishes.Select(fish => fish.ToString()).Contains(GameManager.Target))
        {
            GetTree().CreateTimer(10).Timeout += GameManager.ChangeTarget;
        };
    }
}
