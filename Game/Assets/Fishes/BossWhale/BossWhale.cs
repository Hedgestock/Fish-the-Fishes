using Godot;
using System;

public partial class BossWhale : Fish
{
    [Export]
    int Passes = 3;

    [Export]
    PackedScene BarnacleScene;

    [Export]
    PathFollow2D BarnaclePath;

    bool IsFirstPass = true;

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

            for (int i = 0; i < 100; i++)
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
        GD.Print($"Spawning whale {Flip} {Position}");
    }

    protected override void Despawn()
    {
        GD.Print($"Despawning whale {Flip} {Position}");

        if (Passes > 0)
        {
            Passes--;
            var parent = GetParent();
            Flip = !Flip;
            GetTree().CreateTimer(1).Timeout += () =>
            {
                parent.AddChild(this);
                this._Ready();
            };
            parent.RemoveChild(this);
            GD.Print($" {Flip} passes left {Passes}");
        }
        else
        {
            base.Despawn();
        }
    }
}
