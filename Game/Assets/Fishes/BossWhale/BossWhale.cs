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
        GD.Print(Sound.Playing); 
    }

    private void RemoveBarnacle()
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
