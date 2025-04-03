using Godot;
using WaffleStock;
using System;

public partial class JellyFish : Fish, IPowerup
{
    public override void _Ready()
    {
        Position = new Vector2(GameManager.ScreenSize.X / 2, GameManager.ScreenSize.Y - 50);
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

        // We reset the rotation because it looks better
        Rotation = 0;

        GravityScale = 0.05f;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (Position.Y > GameManager.ScreenSize.Y - 50)
            Push();
        Velocity *= 0.99f;
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
