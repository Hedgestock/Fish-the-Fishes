using Godot;
using System;

public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    [Signal]
    public delegate void EndGameEventHandler();

    private HBoxContainer LivesContainer;
    private GameManager GM;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GM = GetNode<GameManager>("/root/GameManager");

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
        // We need to do this to avoid uint underflow
        if (-score > GM.score) GM.score = 0;
        else GM.score = (uint)((int)GM.score +  score);
        GetNode<Label>("Score").Text = GM.score.ToString();
    }

    private void _on_fishing_line_hit()
    {
        GM.lives--;
        LivesContainer.GetNode<AnimatedSprite2D>("Life" + (3 - GM.lives)).Animation = "death";
        if (GM.lives <= 0)
        {
            GetTree().CreateTimer(1).Timeout += () => { EmitSignal(SignalName.EndGame); };
        }
    }

    private void ResetGame()
    {
        GM.SaveGame();
    }
}


