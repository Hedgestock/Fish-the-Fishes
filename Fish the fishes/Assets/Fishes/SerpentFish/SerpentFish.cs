using Godot;
using System;

public partial class SerpentFish : Fish
{
    [Export]
    Line2D Body;
    [Export]
    uint Length = 50;
    [Export]
    uint SegmentLength = 10;
    [Export]
    Curve AmplitudeCurve;

    private Vector2[] LastPositions;
    private Vector2 LastPosition;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Vector2[] tmp = new Vector2[Length];

        tmp[Length - 1] = Vector2.Zero;
        if (!IsActionable)
        {
            for (int i = 0; i < Length - 2; i++)
            {
                tmp[i + 1] = LastPositions[i] + (LastPosition - GlobalPosition);
                if (Flip)
                    tmp[i + 1].X = LastPositions[i].X - (LastPosition.X - GlobalPosition.X);
            }
        }
        else
        {

            for (int i = 0; i < Length - 1; i++)
            {
                tmp[i] = new Vector2(-SegmentLength * (Length - i - 1), (float)Math.Sin((GlobalPosition.X - (SegmentLength * i)) / 20) * (50 * AmplitudeCurve.Sample((float)i / Length)));
            }
        }
        Body.Points = tmp;
        LastPositions = Body.Points;
        LastPosition = GlobalPosition;
    }
}
