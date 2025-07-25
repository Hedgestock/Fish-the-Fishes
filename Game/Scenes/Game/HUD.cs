using Godot;
using System;
public partial class HUD : CanvasLayer
{
    [Signal]
    public delegate void EndGameEventHandler();

    [Export]
    private Label ScoreLabel;
    [Export]
    private Label ScoreChangeLabel;
    [Export]
    private Label TimeLabel;
    [Export]
    private Timer GameTimer;
    [Export]
    private HBoxContainer LivesContainer;
    [Export]
    private AnimatedSpriteForUI Target;

    [ExportGroup("Sounds")]
    [Export]
    private AudioStream ScoreUp;
    [Export]
    private AudioStream ScoreDown;
    [Export]
    private AudioStream LoseGame;

    private Tween tween;
    private long LocalScore { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LocalScore = GameManager.Score;
        ScoreLabel.Text = GameManager.Score.ToString();

        switch (GameManager.Mode)
        {
            case Game.Mode.Menu:
                break;
            case Game.Mode.TimeAttack:
                TimeLabel.Show();
                GameTimer.Start();
                break;
            case Game.Mode.Target:
                GameManager.Instance.Connect(GameManager.SignalName.TargetChanged, Callable.From(OnTargetChanged));
                GameManager.ChangeTarget();
                Target.GetParent<Control>().Show();
                break;
            case Game.Mode.GoGreen:
                LivesContainer.GetNode<AnimatedSpriteForUI>("Life1").Animation = "life_go_green";
                LivesContainer.GetNode<AnimatedSpriteForUI>("Life2").Animation = "life_go_green";
                LivesContainer.GetNode<AnimatedSpriteForUI>("Life3").Animation = "life_go_green";
                goto default;
            case Game.Mode.Classic:
            case Game.Mode.Zen:
            case Game.Mode.Training:
            default:
                LivesContainer.Show();
                LivesContainer.GetNode<AnimatedSpriteForUI>("Life1").Play();
                LivesContainer.GetNode<AnimatedSpriteForUI>("Life2").Play();
                LivesContainer.GetNode<AnimatedSpriteForUI>("Life3").Play();
                GameManager.Instance.Connect(GameManager.SignalName.LifeUp, new Callable(this, MethodName.LifeUp));
                break;
        }

        for(int i = 1; i <= 3 - GameManager.Lives; i++)
            LivesContainer.GetNode<AnimatedSpriteForUI>("Life" + i).Sprite.Animation = "death";
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
        // We need to do this to avoid uint underflow.
        if (-score > GameManager.Score) GameManager.Score = 0;
        else GameManager.Score = (uint)((int)GameManager.Score + score);

        if (score != 0)
        {
            AchievementsManager.OnPointsScored(score);

            // This is some dark magic to make the score animate
            Tween tweenScore = CreateTween();
            tweenScore.TweenMethod(Callable.From<uint>((s) => ScoreLabel.Text = s.ToString()),
                LocalScore,
                GameManager.Score,
                1);
            LocalScore = GameManager.Score;

            // This is some dark magic to make the little "+X" score thing
            if (tween != null)
            {
                tween.Kill();
                CleanScoreChangeTween();
            }

            tween = CreateTween();
            tween.TweenMethod(Callable.From<int>((fontSize) => ScoreChangeLabel.AddThemeFontSizeOverride("font_size", fontSize)),
                ScoreChangeLabel.GetThemeFontSize("font_size") * 2,
                ScoreChangeLabel.GetThemeFontSize("font_size"),
                0.5f);

            ScoreChangeLabel.Text = score.ToString("+#;-#;0");

            if (score > 0)
            {
                ScoreChangeLabel.AddThemeColorOverride("font_color", new Color(0.12f, 0.6f, 0));
                AudioManager.UIPlay(ScoreUp);
                if (GameManager.Mode == Game.Mode.Target)
                {
                    GameManager.ChangeTarget();
                }
            }
            else if (score < 0)
            {
                AudioManager.UIPlay(ScoreDown);
                ScoreChangeLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.12f, 0));
            }

            tween.TweenMethod(Callable.From<Color>((color) => ScoreChangeLabel.AddThemeColorOverride("font_color", color)),
                ScoreChangeLabel.GetThemeColor("font_color"),
                new Color(ScoreChangeLabel.GetThemeColor("font_color"), 0),
                0.2f);

            ScoreChangeLabel.Show();

            tween.TweenCallback(Callable.From(CleanScoreChangeTween));

            void CleanScoreChangeTween()
            {
                ScoreChangeLabel.Hide();
                ScoreChangeLabel.RemoveThemeColorOverride("font_color");
                ScoreChangeLabel.RemoveThemeFontSizeOverride("font_size");
            }
        }
        else if (GameManager.Mode == Game.Mode.Target)
        {
            EndCurrentGame();
        }

    }

    private void LineHit(FishingLine.DamageType damageType)
    {
        if (GameManager.Mode == Game.Mode.TimeAttack || GameManager.Mode == Game.Mode.Target) return;
        GameManager.Lives--;

        AnimatedSprite2D Life = LivesContainer.GetNode<AnimatedSpriteForUI>("Life" + (3 - GameManager.Lives)).Sprite;
        Life.Animation = "death";

        UpdateLifeSprite(Life);

        if (GameManager.Lives <= 0)
        {
            if (GameManager.Mode == Game.Mode.GoGreen) EndCurrentGame();
            else GetTree().CreateTimer(1).Timeout += EndCurrentGame;
        }
    }

    private void LifeUp()
    {
        if (GameManager.Lives >= 3) return;
        AnimatedSprite2D Life = LivesContainer.GetNode<AnimatedSpriteForUI>("Life" + (3 - GameManager.Lives)).Sprite;
        GameManager.Lives++;

        Life.Animation = GameManager.Mode == Game.Mode.GoGreen ? "life_go_green" : "life";

        UpdateLifeSprite(Life);
    }

    private void UpdateLifeSprite(AnimatedSprite2D sprite)
    {
        sprite.Scale = Vector2.One * 2;
        Vector2 originalPosition = sprite.Position;
        sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + 20);

        Tween tween = CreateTween();
        tween.SetParallel(true);
        tween.TweenProperty(sprite, "scale", Vector2.One, 1).SetTrans(Tween.TransitionType.Elastic);
        tween.TweenProperty(sprite, "position", originalPosition, 1).SetTrans(Tween.TransitionType.Elastic);
    }

    private void EndCurrentGame()
    {
        AudioManager.UIPlay(LoseGame);
        EmitSignal(SignalName.EndGame);
    }

    private void OnTargetChanged()
    {
        string resourcePath = $"res://Game/Assets/Fishes/{GameManager.Target}/Animation/{GameManager.Target}Animation.tres";
        Target.SpriteFrames = GD.Load<SpriteFrames>(resourcePath);

        Target.Play();
    }
}


