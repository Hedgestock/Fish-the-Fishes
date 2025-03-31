using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class Equipment : CanvasLayer
{
    [Export]
    private Node GameContainer;

    [Export]
    private FishingLine FishingLine;

    [Export]
    private Container Menu;

    [Export]
    private PackedScene EquipmentUITemplate;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PopulateHooks();
    }

    private void PopulateHooks()
    {
        foreach (var hook in FishingLine.Hooks)
        {
            var button = EquipmentUITemplate.Instantiate<EquipmentUi>();
            var sprite = hook.Value.Instantiate<AnimatedSprite2D>();
            button.SetUI(hook.Key, EquipmentPiece.Type.Hook, sprite.SpriteFrames, FishingLine.EquipStuff);
            Menu.AddChild(button);
        }
    }

    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Game/Scenes/Home.tscn");
    }

    private void SpawnFish()
    {
        PackedScene FishScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Fishes));
        Fish fish = FishScene.Instantiate<Fish>();

        GameContainer.AddChild(fish);
    }
}
