using Godot;
using System;

public partial class BossWhale : Fish
{
    [Export]
    PackedScene BarnacleScene;

    [Export]
    PathFollow2D BarnaclePath;

    public override void _Ready()
    {
        base._Ready();
        for (int i = 0; i < 100; i++)
        {
            BarnaclePath.ProgressRatio = GD.Randf();
            Barnacle barnacle = BarnacleScene.Instantiate<Barnacle>();
            barnacle.Position = BarnaclePath.Position;
            AddChild(barnacle);
        }
    }
}
