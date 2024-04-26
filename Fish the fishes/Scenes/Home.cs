using Godot;
using Godot.Collections;
using System;

public partial class Home : CanvasLayer
{
    [Export]
    private Label Message;

    [Export]
    public Array<PackedScene> Fishes;

    [Export]
    public Node GameContainer;

    private GameManager GM;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GM = GetNode<GameManager>("/root/GameManager");

        if (GameManager.PrevScene == "res://SplashScreen.tscn")
        {
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
        }
        else
        {
            Message.Text = "Last Score:\n" + GameManager.Score.ToString();
        }
    }

    private void Play(Game.Mode mode)
    {
        GameManager.Mode = mode;
        GM.ChangeSceneToFile("res://Fish the fishes/Scenes/Game/Game.tscn");
    }

    private void PlayClassic()
    {
        Play(Game.Mode.Classic);
    }

    private void PlayTimeAttack()
    {
        Play(Game.Mode.TimeAttack);
    }

    private void PlayGoGreen()
    {
        Play(Game.Mode.GoGreen);
    }

    private void PlayTarget()
    {
        Play(Game.Mode.Target);
    }

    private void PlayZen()
    {
        Play(Game.Mode.Zen);
    }

    private void PlayTest()
    {
        GameManager.Mode = Game.Mode.Classic;
        GM.ChangeSceneToFile("res://Fish the fishes/Scenes/Game/GameTest.tscn");
    }

    private void GoToSettings()
    {
        GM.ChangeSceneToFile("res://Fish the fishes/Scenes/Settings/Settings.tscn");
    }

    private void GoToCompendium()
    {
        GM.ChangeSceneToFile("res://Fish the fishes/Scenes/Compendium/Compendium.tscn");
    }

    private void GoToCredits()
    {
        GM.ChangeSceneToFile("res://Fish the fishes/Scenes/Credits/Credits.tscn");
    }

    private void GoToStats()
    {
        GM.ChangeSceneToFile("res://Fish the fishes/Scenes/Stats/Stats.tscn");
    }

    private void GoToTutorial()
    {
        GM.ChangeSceneToFile("res://Fish the fishes/Scenes/Tutorial/Tutorial.tscn");
    }

    private void SpawnFish()
    {
        PackedScene FishScene = Fishes[(int)(GD.Randi() % Fishes.Count)];
        Fish fish = FishScene.Instantiate<Fish>();

        bool flip = (GD.Randi() % 2) != 0;
        Vector2 fishSpawnLocation = new Vector2(flip ? GameManager.ScreenSize.X + 200 : -200, (float)GD.RandRange(0, GameManager.ScreenSize.Y));
        fish.Position = fishSpawnLocation;
        fish.Flip = flip;

        GameContainer.AddChild(fish);
        
    }
}
