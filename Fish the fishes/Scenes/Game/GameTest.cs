using Godot;
using Godot.Collections;
using System;

public partial class GameTest : Node
{
    [Export]
    public Array<PackedScene> Fishes { get; set; }

    [Export]
    public Array<PackedScene> Trashes { get; set; }

    private GameManager gameManager;

    public enum Mode
    {
        Classic,
        GoGreen,
        Training,
        TimeAttack,
        Zen
    }

    private Vector2 ScreenSize;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ScreenSize = GetViewport().GetVisibleRect().Size;

        gameManager = GetNode<GameManager>("/root/GameManager");
        gameManager.Score = 0;
        gameManager.Lives = 3;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void EndGame()
    {
        gameManager.SaveGame();
        gameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }
    private int num = 0;
    private void SpawnFish()
    {
        PackedScene FishScene = Fishes[num++ % Fishes.Count];
        Fish fish = FishScene.Instantiate<Fish>();

        bool flip = (GD.Randi() % 2) != 0;
        Vector2 fishSpawnLocation = new Vector2(flip ? ScreenSize.X + 200 : -200, (float)GD.RandRange(0, ScreenSize.Y));
        fish.Position = fishSpawnLocation;
        fish.Flip = flip;

        // spawn the mob by adding it to the main scene.
        AddChild(fish);
    }

    private void Despawn(Node2D body)
    {
        if (body is Fish && (body as Fish).IsCaught) return;
        body.QueueFree();
    }

    private void SpawnTrash()
    {
        PackedScene TrashScene = Trashes[(int)(GD.Randi() % Trashes.Count)];
        Trash trash = TrashScene.Instantiate<Trash>();
        Vector2 trashSpawnLocation = new Vector2(GD.Randi() % ScreenSize.X, -50);
        trash.Position = trashSpawnLocation;
        trash.LinearVelocity = new Vector2(GD.RandRange(-100, 100), 0);

        // Spawn the mob by adding it to the Main scene.
        AddChild(trash);
    }
}