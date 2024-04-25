using Godot;
using System;
using System.Reflection.Metadata;

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
            LivesContainer.GetNode<AnimatedSpriteForUI>("Life1").Play();
            LivesContainer.GetNode<AnimatedSpriteForUI>("Life2").Play();
            LivesContainer.GetNode<AnimatedSpriteForUI>("Life3").Play();
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

        if (score != 0)
        {
            ScoreLabel.Scale = Vector2.One * 1.5f;

            Tween tween = CreateTween();
            tween.TweenProperty(ScoreLabel, "scale", Vector2.One, 1).SetTrans(Tween.TransitionType.Elastic);

            if (score > 0)
            {
                ScoreLabel.AddThemeColorOverride("font_color", new Color(0.12f, 0.6f, 0));

            }
            else if (score < 0)
            {
                ScoreLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.12f, 0));
            }

            tween.TweenCallback(Callable.From(() => ScoreLabel.RemoveThemeColorOverride("font_color")));
        }

    }

    private void LineHit(FishingLine.DamageType damageType)
    {
        if (GM.Mode == Game.Mode.TimeAttack) return;
        GM.Lives--;

        AnimatedSprite2D Life = LivesContainer.GetNode<AnimatedSprite2D>("Life" + (3 - GM.Lives));
        Life.Animation = "death";

        Life.Scale = Vector2.One;
        Vector2 originalPosition = Life.Position;
        Life.Position = new Vector2(Life.Position.X, Life.Position.Y + 20);

        Tween tween = CreateTween();
        tween.SetParallel(true);
        tween.TweenProperty(Life, "scale", Vector2.One * 0.5f, 1).SetTrans(Tween.TransitionType.Elastic);
        tween.TweenProperty(Life, "position", originalPosition, 1).SetTrans(Tween.TransitionType.Elastic);

        if (GM.Lives <= 0)
        {
            if (GM.Mode == Game.Mode.GoGreen) EndCurrentGame();
            else GetTree().CreateTimer(1).Timeout += EndCurrentGame;
        }
    }

    private void EndCurrentGame()
    {
        EmitSignal(SignalName.EndGame);
    }
}


