using Godot;
using System.Linq;
using WaffleStock;

public partial class BossWhale : Boss
{

    [Export]
    PackedScene BarnacleScene;

    [Export]
    PathFollow2D BarnaclePath;

    int BarnaclesLeft;
    int BarnaclesMax;

    public override void _Ready()
    {
        base._Ready();
        BarnaclesLeft = BarnaclesMax = GD.RandRange(80, 120);

        for (int i = 0; i < BarnaclesMax; i++)
        {
            BarnaclePath.ProgressRatio = GD.Randf();
            Barnacle barnacle = BarnacleScene.Instantiate<Barnacle>();
            barnacle.Position = BarnaclePath.Position;
            AddChild(barnacle);
        }
    }

    protected override void PreparePass()
    {
        base.PreparePass();

        Rotation = 0;

        if (Flip)
            Scale = new(-1, 1);
        else
            Scale = new(1, 1);

        Scale *= ActualSizeVariation;

        float positionOffset = VisibleOnScreenNotifier.Rect.Size.X * VisibleOnScreenNotifier.Scale.X / 2 * ActualSizeVariation;
        Position = new Vector2(
            Flip ? GameManager.ScreenSize.X + positionOffset : -positionOffset,
            GameManager.ScreenSize.Y / 2
        );

        TravelAxis = Flip ? Vector2.Left : Vector2.Right;

        Velocity = TravelAxis * ActualSpeed;
    }

    public void RemoveBarnacle()
    {
        BarnaclesLeft--;
        if (BarnaclesLeft <= BarnaclesMax * 0.2)
        {
            foreach (var barnacle in GetChildren().OfType<Barnacle>())
            {
                barnacle.Kill();
            }

            int score = 0;

            switch (GameManager.Mode)
            {
                case Game.Mode.Classic:
                case Game.Mode.TimeAttack:
                    score = Scoring.ClassicScore([this], false);
                    break;
            }

            GetNode("../FishingLine").EmitSignal(FishingLine.SignalName.Score, score);

            Passes = 0;
        }
    }
}
