using Godot;
using System;

public partial class Equipment : CanvasLayer
{
    [Export]
    private Node GameContainer;

    [Export]
    private FishingLine FishingLine;

    [Export]
    private VBoxContainer Menu;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PopulateHooks();
    }

    public void Test() {
        FishingLine._Ready();
    }

    private void PopulateHooks()
    {
        foreach (var hook in FishingLine.Hooks)
        {
            var test = new AnimatedSpriteForUI();
            var instance = hook.Instantiate<AnimatedSprite2D>();
            test.SpriteFrames = instance.SpriteFrames;
            Menu.AddChild(test);
        }
    }

    private void GoToHome()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Home.tscn");
    }

    private void SpawnFish()
    {
        PackedScene FishScene = GD.Load<PackedScene>(Biome.GetRandomPathFrom(GameManager.Biome.Fishes));
        Fish fish = FishScene.Instantiate<Fish>();

        GameContainer.AddChild(fish);
    }
}
