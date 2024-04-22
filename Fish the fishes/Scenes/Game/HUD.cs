using Godot;
using System;

public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void EndGameEventHandler();

    private Label ScoreLabel;
    private Label TimeLabel;
    private Timer GameTimer;
    private HBoxContainer LivesContainer;
    private GameManager GM;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GM = GetNode<GameManager>("/root/GameManager");

        LivesContainer = GetNode<HBoxContainer>("Lives");
        ScoreLabel = GetNode<Label>("Score");
        TimeLabel = GetNode<Label>("Time");
        GameTimer = GetNode<Timer>("GameTimer");

        if (GM.Mode == Game.Mode.TimeAttack)
        {
            TimeLabel.Show();
            GameTimer.Start();
        }
        else
        {
            LivesContainer.Show();
            LivesContainer.GetNode<AnimatedSprite2D>("Life1").Play();
            LivesContainer.GetNode<AnimatedSprite2D>("Life2").Play();
            LivesContainer.GetNode<AnimatedSprite2D>("Life3").Play();
        }

    }

    public override void _Process(double delta)
    {
        if (GM.Mode == Game.Mode.TimeAttack)
        {
            TimeLabel.Text = ((int)GameTimer.TimeLeft).ToString();
        }
    }

    private void LineScore(int score)
    {
        // We need to do this to avoid uint underflow
        if (-score > GM.Score) GM.Score = 0;
        else GM.Score = (uint)((int)GM.Score + score);
        ScoreLabel.Text = GM.Score.ToString();
    }

    private void LineHit(FishingLine.DamageType damageType)
    {
        if (GM.Mode == Game.Mode.TimeAttack) return;
        GM.Lives--;
        LivesContainer.GetNode<AnimatedSprite2D>("Life" + (3 - GM.Lives)).Animation = "death";
        if (GM.Lives <= 0)
        {
            if (GM.Mode == Game.Mode.GoGreen)   EndCurrentGame();
            else GetTree().CreateTimer(1).Timeout += EndCurrentGame;
        }
    }

    private void EndCurrentGame()
    {
        EmitSignal(SignalName.EndGame);
    }
}


