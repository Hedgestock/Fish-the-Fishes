using Godot;
using Godot.Collections;
using System;

public partial class Game : Node
{
	[Export]
	public Array<PackedScene> Fishes { get; set; }

	[Export]
	public Array<PackedScene> Trashes { get; set; }

	private GameManager GM;
	private RectangleShape2D PlayingZone;

	public enum Mode
	{
		Classic,
		GoGreen,
		Training,
		TimeAttack,
		Zen
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GM = GetNode<GameManager>("/root/GameManager");
		GM.Score = 0;
		GM.Lives = 3;

		PlayingZone = (RectangleShape2D)GetNode<CollisionShape2D>("PlayingZone/Area2D/CollisionShape2D").Shape;

		PlayingZone.Size = new Vector2(GM.ScreenSize.X + 400, GM.ScreenSize.Y + 400);

		GetTree().Root.SizeChanged += OnScreenResize;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void EndGame()
	{
		GM.WriteHighScore();
		GM.SaveData();
		GM.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
	}

	private void SpawnFish()
	{
		PackedScene FishScene = Fishes[(int)(GD.Randi() % Fishes.Count)];
		Fish fish = FishScene.Instantiate<Fish>();

		bool flip = (GD.Randi() % 2) != 0;
		Vector2 fishSpawnLocation = new Vector2(flip ? GM.ScreenSize.X + 200 : -200, (float)GD.RandRange(0, GM.ScreenSize.Y));
		fish.Position = fishSpawnLocation;
		fish.Flip = flip;

		// Spawn the fish by adding it to the main scene.
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
		Vector2 trashSpawnLocation = new Vector2(GD.Randi() % GM.ScreenSize.X, -50);
		trash.Position = trashSpawnLocation;
		trash.LinearVelocity = new Vector2(GD.RandRange(-200, 200), 0);

		// Spawn the trash by adding it to the Main scene.
		AddChild(trash);
	}

	private void OnScreenResize()
	{
		PlayingZone.Size = new Vector2(GM.ScreenSize.X + 400, GM.ScreenSize.Y + 400);
	}
}
