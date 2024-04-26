using Godot;
using System;
using System.Reflection.Metadata;

public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void EndGameEventHandler();

    [Signal]
    public delegate void TargetFishedEventHandler();

    [Export]
    private Label ScoreLabel;
    [Export]
    private Label TimeLabel;
    [Export]
    private Timer GameTimer;
    [Export]
    private HBoxContainer LivesContainer;
    [Export]
    private AnimatedSpriteForUI Target;

    private GameManager GM;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GM = GetNode<GameManager>("/root/GameManager");

        switch (GameManager.Mode)
        {
            case Game.Mode.Menu:
                break;
            case Game.Mode.TimeAttack:
                TimeLabel.Show();
                GameTimer.Start();
                break;
            case Game.Mode.Target:
                GM.Connect(GameManager.SignalName.TargetChanged, Callable.From(ChangeTarget));
                Target.GetParent<Control>().Show();
                break;
            case Game.Mode.Classic:
            case Game.Mode.GoGreen:
            case Game.Mode.Zen:
            case Game.Mode.Training:
            default:
                LivesContainer.Show();
                LivesContainer.GetNode<AnimatedSpriteForUI>("Life1").Play();
                LivesContainer.GetNode<AnimatedSpriteForUI>("Life2").Play();
                LivesContainer.GetNode<AnimatedSpriteForUI>("Life3").Play();
                break;
        }
    }

    public override void _Process(double delta)
    {
        if (GameManager.Mode == Game.Mode.TimeAttack)
        {
            TimeLabel.Text = ((int)GameTimer.TimeLeft).ToString();
        }
    }

    private void LineScore(int score)
    {
        // We need to do this to avoid uint underflow
        if (-score > GameManager.Score) GameManager.Score = 0;
        else GameManager.Score = (uint)((int)GameManager.Score + score);
        ScoreLabel.Text = GameManager.Score.ToString();

        if (score != 0)
        {
            Tween tween = CreateTween();

            if (score > 0)
            {
                ScoreLabel.AddThemeColorOverride("font_color", new Color(0.12f, 0.6f, 0));
                if (GameManager.Mode == Game.Mode.Target)
                {
                    EmitSignal(SignalName.TargetFished);
                }
            }
            else if (score < 0)
            {
                ScoreLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.12f, 0));
            }

            tween.TweenCallback(Callable.From(() => ScoreLabel.RemoveThemeColorOverride("font_color")));
        }
        else if (GameManager.Mode == Game.Mode.Target)
        {
            EndCurrentGame();
        }

    }

    private void LineHit(FishingLine.DamageType damageType)
    {
        if (GameManager.Mode == Game.Mode.TimeAttack) return;
        GameManager.Lives--;

        AnimatedSprite2D Life = LivesContainer.GetNode<AnimatedSpriteForUI>("Life" + (3 - GameManager.Lives)).Sprite;
        Life.Animation = "death";

        Life.Scale = Vector2.One * 2;
        Vector2 originalPosition = Life.Position;
        Life.Position = new Vector2(Life.Position.X, Life.Position.Y + 20);

        Tween tween = CreateTween();
        tween.SetParallel(true);
        tween.TweenProperty(Life, "scale", Vector2.One, 1).SetTrans(Tween.TransitionType.Elastic);
        tween.TweenProperty(Life, "position", originalPosition, 1).SetTrans(Tween.TransitionType.Elastic);

        if (GameManager.Lives <= 0)
        {
            if (GameManager.Mode == Game.Mode.GoGreen) EndCurrentGame();
            else GetTree().CreateTimer(1).Timeout += EndCurrentGame;
        }
    }

    private void EndCurrentGame()
    {
        EmitSignal(SignalName.EndGame);
    }

    private void ChangeTarget()
    {
        string resourcePath = $"res://Fish the fishes/Assets/Fishes/{GM.Target}/{GM.Target}Animation.tres";
        Target.SpriteFrames = GD.Load<SpriteFrames>(resourcePath);
        Target.Play();
    }
}


