using Godot;
using Godot.Collections;
using Godot.Fish_the_fishes.Scripts;
using System;

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

        if (GameManager.Mode == Mode.Target)
        {
            ChangeTarget();
        }
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
        PackedScene FishScene = GameManager.Biome.Fishes[(int)(GD.Randi() % GameManager.Biome.Fishes.Count)].Item;
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
        PackedScene TrashScene = GameManager.Biome.Trashes[(int)(GD.Randi() % GameManager.Biome.Trashes.Count)].Item;
        Trash trash = TrashScene.Instantiate<Trash>();
        Vector2 trashSpawnLocation = new Vector2(GD.Randi() % GameManager.ScreenSize.X, -100);
        trash.Position = trashSpawnLocation;
        trash.Velocity = new Vector2(GD.RandRange(-200, 200), 0);

        // Spawn the trash by adding it to the Main scene.
        AddChild(trash);
    }

    private void ChangeTarget()
    {
        // TO FIX
        GameManager.Target = Biome.ChooseFrom(GameManager.Biome.Fishes).Instantiate<Fish>().GetType().Name;
    }

    private void ChangeBiome()
    {
        if (GameManager.Biome.FollowupBiomes.Count == 0) return;
        GameManager.Biome = GameManager.Biome.FollowupBiomes[(int)(GD.Randi() % GameManager.Biome.FollowupBiomes.Count)];
        Background.Texture = GameManager.Biome.Background;
    }
}
