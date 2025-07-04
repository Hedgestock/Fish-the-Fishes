using Godot;
using WaffleStock;
using System.Collections.Generic;
using System.Linq;
using System;

public partial class JellyFish : Fish, IPowerup
{
    [Export]
    Node2D Tentacles;
    List<Line2D> TentaclesList;

    Vector2 LastPosition;

    public override void _Ready()
    {
        TentaclesList = Tentacles.GetChildren().OfType<Line2D>().ToList();
        foreach (var tentacle in TentaclesList)
        {
            List<Vector2> tmp = new();
            for (int i = 0; i < 10; i++)
            {
                tmp.Add(new Vector2(0, i * tentacle.Points[1].Y / 10));
            }
            tentacle.Points = tmp.ToArray();
            //tentacle.Points = Enumerable.Repeat(new Vector2(), 100).ToArray();
        }

        Position = new Vector2(GameManager.ScreenSize.X / 2, GameManager.ScreenSize.Y + 50);
        Flip = (GD.Randi() % 2) != 0;

        Vector2 objective = new Vector2(
            !Flip ? GameManager.ScreenSize.X : 0,
            (float)GD.RandRange(
                GameManager.ScreenSize.Y * SpawnRange.X,
                GameManager.ScreenSize.Y * SpawnRange.Y
                )
            );
        TravelAxis = (objective - Position).Normalized();

        base._Ready();

        LastPosition = Position;

        // We reset the rotation and scale (flip) because it looks better
        Rotation = 0;
        Scale = new Vector2(1, 1);

        GravityScale = 0.05f;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Position.Y > GameManager.ScreenSize.Y)
            Push();
        Velocity *= 0.99f;

        foreach (var tentacle in TentaclesList)
        {
            Vector2[] tmp = tentacle.Points;

            for (int i = tmp.Length - 1; i > 0; i--)
            {
                float displacement = MathF.Sqrt(Math.Abs(tentacle.Position.X));
                if (tentacle.Position.X > 0)
                {
                    if (Velocity.Y > 0)
                        tmp[i] = new Vector2(Math.Min(tmp[i].X + tentacle.Position.X / 100, i * displacement), tmp[i].Y);
                    else
                        tmp[i] = new Vector2(Math.Max(tmp[i].X - tentacle.Position.X / 10, 0), tmp[i].Y);
                }
                else
                {
                    displacement *= -1;
                    if (Velocity.Y > 0)
                        tmp[i] = new Vector2(Math.Max(tmp[i].X + tentacle.Position.X / 100, i * displacement), tmp[i].Y);
                    else
                        tmp[i] = new Vector2(Math.Min(tmp[i].X - tentacle.Position.X / 10, 0), tmp[i].Y);
                }
                tmp[i].X += (float)GD.RandRange(-0.5, 0.5);
            }
            tentacle.Points = tmp.ToArray();
        }
        LastPosition = Position;

        base._PhysicsProcess(delta);
    }

    public void Activate()
    {
        GameManager.Instance.EmitSignal(GameManager.SignalName.LifeUp);
    }

    private void Push()
    {
        if (IsActionable)
        {
            Velocity = TravelAxis * ActualSpeed;
        }
    }
}
