using Godot;
using Godot.Collections;
using System;

public partial class FTFMain : Node
{
	[Export]
	public Array<PackedScene> Fishes { get; set; }

	[Export]
	public Array<PackedScene> Trashes { get; set; }


	private Vector2 ScreenSize;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewport().GetVisibleRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void StartGame() {
		GetNode<Timer>("FishTimer").Start();
		GetNode<Timer>("TrashTimer").Start();
		GetNode<FishingLine>("FishingLine").Show();
	}

	public void EndGame()
	{
		GetTree().CallGroup("items", Node.MethodName.QueueFree);
		GetNode<Timer>("FishTimer").Stop();
		GetNode<Timer>("TrashTimer").Stop();
		GetNode<FishingLine>("FishingLine").Hide();
	}

	private void _on_fish_timer_timeout()
	{
		PackedScene FishScene = Fishes[(int) (GD.Randi() % Fishes.Count)];
		Fish fish = FishScene.Instantiate<Fish>();

		bool flip = (GD.Randi() % 2) != 0;
		Vector2 fishSpawnLocation = new Vector2(flip? -100 : ScreenSize.X + 100,(float) GD.RandRange(0,  ScreenSize.Y));
		fish.Position = fishSpawnLocation;


		if (flip)
		{
			fish.LinearVelocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
		} else
		{
			fish.LinearVelocity = new Vector2((float)GD.RandRange(-150.0, -250.0), 0);
		}

		fish.GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = flip;


		// Spawn the mob by adding it to the Main scene.
		AddChild(fish);
	}
	
	private void _on_trash_timer_timeout()
	{
		PackedScene TrashScene = Trashes[(int) (GD.Randi() % Trashes.Count)];
		Trash trash = TrashScene.Instantiate<Trash>();
		Vector2 trashSpawnLocation = new Vector2(GD.Randi() % ScreenSize.X, - 50);
		trash.Position = trashSpawnLocation;
		trash.LinearVelocity = new Vector2(GD.RandRange(-100, 100), 0);

		// Spawn the mob by adding it to the Main scene.
		AddChild(trash);
	}
}
