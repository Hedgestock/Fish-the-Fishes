using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using WaffleStock;

public partial class BossWhale : Fish
{

    [Export]
    PackedScene BarnacleScene;

    [Export]
    PathFollow2D BarnaclePath;

    bool IsFirstPass = true;
    int Passes;
    int BarnaclesLeft;
    int BarnaclesMax;

    public override void _Ready()
    {
        if (IsFirstPass)
        {
            if (GameManager.Flip != 0)
                Flip = GameManager.Flip > 0;
            else
                Flip = (GD.Randi() % 2) != 0;

            Sprite.Animation = "alive";
            Sprite.Play();

            if (IsInDisplay) return;
            NotifySpawn();

            Passes = GD.RandRange(3, 5);

            BarnaclesLeft = BarnaclesMax = GD.RandRange(80, 120);

            for (int i = 0; i < BarnaclesMax; i++)
            {
                BarnaclePath.ProgressRatio = GD.Randf();
                Barnacle barnacle = BarnacleScene.Instantiate<Barnacle>();
                barnacle.Position = BarnaclePath.Position;
                AddChild(barnacle);
            }

            if (GameManager.Mode != Game.Mode.Menu)
            {
                Sound.Play();
            }

            IsCaught = false;

            ActualSizeVariation = (float)Mathf.Max(0.01, GD.Randfn(1, SizeDeviation));

            ActualSpeed = (float)GD.RandRange(MinSpeed, MaxSpeed);

            IsFirstPass = false;
        }

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

    protected override void Despawn()
    {
        if (Passes > 0)
        {
            Passes--;
            var parent = GetParent();
            Flip = !Flip;
            GetTree().CreateTimer(GD.RandRange(3, 12)).Timeout += () =>
            {
                parent.AddChild(this);
                this._Ready();
            };
            parent.RemoveChild(this);
        }
        else
        {
            base.Despawn();
        }
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
        }
    }
}
