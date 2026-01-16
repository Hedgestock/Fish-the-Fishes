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
            barnacle.Connect(Fish.SignalName.GotFished, Callable.From((Node by) => RemoveBarnacle()),
                (uint)ConnectFlags.OneShot);

            AddChild(barnacle);
        }
    }

    private void RemoveBarnacle()
    {
        BarnaclesLeft--;
        GD.Print($"Barnacles left {BarnaclesLeft} {BarnaclesMax * 0.2}");
        if (!IsCaught && BarnaclesLeft <= BarnaclesMax * 0.2)
        {
            IsCaught = true;

            int score = 0;

            switch (GameManager.Mode)
            {
                case Game.Mode.Training:
                case Game.Mode.Classic:
                case Game.Mode.TimeAttack:
                    score = Scoring.ClassicScore([this], false);
                    break;
            }

            GetNode("../FishingLine").EmitSignal(FishingLine.SignalName.Score, score);

            foreach (var barnacle in GetChildren().OfType<Barnacle>())
            {
                barnacle.Kill();
            }

            GD.Print($"scoring whalw {score}");

            Passes = 0;
        }
    }
}
