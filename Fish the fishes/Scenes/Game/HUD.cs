using Godot;
using System;

public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    [Signal]
    public delegate void EndGameEventHandler();

    private HBoxContainer LivesContainer;
    private GameManager gameManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameManager = GetNode<GameManager>("/root/GameManager");

        LivesContainer = GetNode<HBoxContainer>("Lives");
        LivesContainer.GetNode<AnimatedSprite2D>("Life1").Play();
        LivesContainer.GetNode<AnimatedSprite2D>("Life2").Play();
        LivesContainer.GetNode<AnimatedSprite2D>("Life3").Play();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void _on_fishing_line_score(int score)
    {
        GD.Print("scoring: ",  score);
        // We need to do this to avoid uint underflow
        if (-score > gameManager.score) gameManager.score = 0;
        else gameManager.score = (uint)((int)gameManager.score +  score);
        GetNode<Label>("Score").Text = gameManager.score.ToString();
    }

    private void _on_fishing_line_hit()
    {
        gameManager.lives--;
        LivesContainer.GetNode<AnimatedSprite2D>("Life" + (3 - gameManager.lives)).Animation = "death";
        if (gameManager.lives <= 0)
        {
            GetTree().CreateTimer(1).Timeout += () => { EmitSignal(SignalName.EndGame); };
        }
    }

    private void ResetGame()
    {
        gameManager.SaveGame();
    }
}


