using Godot;
using System;

public partial class Home : CanvasLayer
{
    private GameManager GM;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        GM = GetNode<GameManager>("/root/GameManager");

        GetNode<Label>("Messages/HighScores/Classic").Text = "Classic High Score: \n" + GM.ClassicHighScore.ToString();
        GetNode<Label>("Messages/HighScores/TimeAttack").Text = "Time Attack High Score: \n" + GM.TimeAttackHighScore.ToString();

        if (GM.PrevScene == "res://SplashScreen.tscn")
        {
            GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();

        } else
        {
            GetNode<Label>("Messages/Message").Text = "Last Score: \n" + GM.Score.ToString();
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

    private void GoToSettings()
    {
        GM.ChangeSceneToFile("res://Fish the fishes/Scenes/Settings/Settings.tscn");
    }

    private void GoToCompendium()
    {
        GM.ChangeSceneToFile("res://Fish the fishes/Scenes/Compendium/Compendium.tscn");
    }
}
