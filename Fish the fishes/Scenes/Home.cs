using Godot;
using Godot.Collections;
using System;

public partial class Home : CanvasLayer
{
    [Export]
    private Label Message;
    [Export]
    private Node GameContainer;
    [Export]
    private TextureRect Background;

    [Export]
    private Biome StartingBiome;
    [Export]
    private Biome TestBiome;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (GameManager.Biome == null) GameManager.Biome = StartingBiome;
        if (GameManager.PrevScene == "res://SplashScreen.tscn")
        {
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        }
        else
        {
            Message.Text = "Last Score:\n" + GameManager.Score.ToString();
        }
        Background.Texture = GameManager.Biome.Background;
    }

    private void Play(Game.Mode mode, Biome biome)
    {
        GameManager.Mode = mode;
        GameManager.Biome = biome;
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Game/Game.tscn");
    }

    private void PlayClassic()
    {
        Play(Game.Mode.Classic, StartingBiome);
    }

    private void PlayTimeAttack()
    {
        Play(Game.Mode.TimeAttack, StartingBiome);
    }

    private void PlayGoGreen()
    {
        Play(Game.Mode.GoGreen, StartingBiome);
    }

    private void PlayTarget()
    {
        Play(Game.Mode.Target, StartingBiome);
    }

    private void PlayTest()
    {
        Play(Game.Mode.Target, TestBiome);
    }

    private void GoToSettings()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Settings/Settings.tscn");
    }

    private void GoToCompendium()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Compendium/Compendium.tscn");
    }

    private void GoToCredits()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Credits/Credits.tscn");
    }

    private void GoToStats()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Stats/Stats.tscn");
    }

    private void GoToTutorial()
    {
        GameManager.ChangeSceneToFile("res://Fish the fishes/Scenes/Tutorial/Tutorial.tscn");
    }

    private void SpawnFish()
    {
        PackedScene FishScene = Biome.ChooseFrom(GameManager.Biome.Fishes) as PackedScene;
        Fish fish = FishScene.Instantiate<Fish>();

        GameContainer.AddChild(fish);
    }
}
