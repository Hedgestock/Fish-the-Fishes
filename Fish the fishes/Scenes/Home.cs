using Godot;
using Godot.Fish_the_fishes.Scripts;
using System;

public partial class Home : CanvasLayer
{
    [Export]
    private Label Message;

    private GameManager GM;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GM = GetNode<GameManager>("/root/GameManager");

        if (GM.PrevScene == "res://SplashScreen.tscn")
        {
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();

        }
        else
        {
            Message.Text = "Last Score:\n" + GM.Score.ToString();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void Play(Game.Mode mode)
    {
        GM.Mode = mode;
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
        GM.Mode = Game.Mode.Classic;
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
}
