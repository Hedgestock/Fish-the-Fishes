using Godot;
using System;

public partial class SardineFish : Fish
{
    [Export]
    public int shoalRadius = 20;
    [Export]
    public int shoalSize = 6;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
        SpawnShoalMember();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void SpawnShoalMember()
    {
        
        if (GD.Randi() % shoalSize == 0) return;

        int offsetX = ((int)GD.Randi() % shoalRadius) - shoalRadius/2;
        int offsetY = ((int)GD.Randi() % shoalRadius) - shoalRadius/2;

        Fish fish = ResourceLoader.Load<PackedScene>(SceneFilePath).Instantiate() as Fish;

        Vector2 fishSpawnLocation = new Vector2(Position.X + offsetX, Position.Y + offsetY);

        fish.Position = fishSpawnLocation;
        fish.LinearVelocity = LinearVelocity;
        fish.flip = flip;
        // spawn the mob by adding it to the main scene.
        GetTree().CurrentScene.AddChild(fish);
    }
}
