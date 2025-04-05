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
        if (IsInDisplay) return;
        Sprite.SpeedScale = GD.Randf() + 0.5f;
        float modulation = (float)GD.RandRange(0.8, 1.2);
        Sprite.SelfModulate = new Color(modulation, modulation, modulation, 1);
        SpawnShoalMember();
    }

    private void SpawnShoalMember()
    {

        if (GD.Randi() % shoalSize == 0) return;

        float newPosX = Position.X + (GD.Randi() % shoalRadius) - shoalRadius / 2;
        float newPosY = Position.Y + (GD.Randi() % shoalRadius) - shoalRadius / 2;

        if (newPosX > -20 && newPosX < GetViewport().GetVisibleRect().Size.X + 20)
            newPosX = Flip ? -20 : GetViewport().GetVisibleRect().Size.X + 20;

        Fish fish = ResourceLoader.Load<PackedScene>(SceneFilePath).Instantiate() as Fish;

        Vector2 fishSpawnLocation = new Vector2(newPosX, newPosY);

        // That ensures the shoal behaves in coordination.
        fish.Position = fishSpawnLocation;
        fish.ActualSpeed = ActualSpeed;
        fish.TravelAxis = TravelAxis;
        fish.Flip = Flip;
        // Spawn the mob by adding it to the game node.
        GetParent().AddChild(fish);
    }
}
