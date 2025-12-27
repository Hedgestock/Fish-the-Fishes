using Godot;
using WaffleStock;
using System;
using System.Collections.Generic;

public partial class SerpentFish : Fish
{
    [Export]
    Line2D Body;

    uint Length;
    uint SegmentLength = 10;

    [Export]
    Curve AmplitudeCurve;
    int WaveAmplitude = 60;
    int WaveLength = 5;

    private Dictionary<int, CollisionShape2D> HurtBoxes = new();

    private DateTime InstanciationTime = DateTime.Now;

    private Node2D AnchorPoint = null;
    private Vector2 AnchorPointLastPosition = Vector2.Zero;
    private int CaughtHurtBoxIndex = 0;

    public override float ActualSize
    {
        get { return (SegmentLength * Length / 4) * ActualSizeVariation; }
    }

    public override void _Ready()
    {
        float pixels = Math.Abs(Body.Points[Body.Points.Length - 1].X / SegmentLength);
        Length = IsInDisplay ? (uint)pixels : (uint)Mathf.Max(SegmentLength * 3, GD.Randfn(pixels, pixels * SizeDeviation));

        VisibleOnScreenNotifier.Rect = new Rect2(-30 - SegmentLength * Length, -WaveAmplitude, 30 + SegmentLength * Length, WaveAmplitude * 2);
        VisibleOnScreenNotifier.Scale = Vector2.One;

        base._Ready();

        GD.Print(ActualSize);

        HurtBoxes[0] = GetNode<CollisionShape2D>("HurtBox");

        int lasti = 0;
        for (int i = 1; i < Length; i++)
        {
            float currentRadius = Body.Width * Body.WidthCurve.Sample((float)i / Length) / 2;
            if ((i - lasti) * SegmentLength > currentRadius)
            {
                CollisionShape2D shape = new();
                shape.Shape = new CircleShape2D();

                (shape.Shape as CircleShape2D).Radius = currentRadius;

                shape.AddToGroup("Hurtboxes");
                HurtBoxes[i] = shape;
                AddChild(shape);
                lasti = i;
            }
        }

        Body.Points = new Vector2[Length];
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (IsCaught)
        {
            GetDragged();
            return;
        }

        if (!IsActionable && !IsInDisplay) return;

        Slither();
    }

    private void Slither()
    {
        Vector2[] tmp = new Vector2[Length];

        for (int i = 0; i < Length; i++)
        {
            tmp[i] = new Vector2(-SegmentLength * i,
                (float)Math.Sin((((DateTime.Now - InstanciationTime).TotalMilliseconds / 1000f) - (SegmentLength * (Length - i))) * WaveLength) * (WaveAmplitude * AmplitudeCurve.Sample((float)i / Length)));
            if (HurtBoxes.ContainsKey(i))
            {
                HurtBoxes[i].Position = tmp[i];
            }
        }

        Body.Points = tmp;
    }

    private void GetDragged()
    {
        Vector2 velocity = (AnchorPoint.GlobalPosition - AnchorPointLastPosition).Rotated(-GlobalRotation);
        if (Flip)
            velocity = new Vector2(velocity.X, -velocity.Y);
        AnchorPointLastPosition = AnchorPoint.GlobalPosition;

        Vector2[] tmp = new Vector2[Length];

        tmp[CaughtHurtBoxIndex] = Body.Points[CaughtHurtBoxIndex];

        for (int i = 1; i <= CaughtHurtBoxIndex || i < Length - CaughtHurtBoxIndex; i++)
        {
            int tail = CaughtHurtBoxIndex - i;
            if (tail >= 0)
            {
                tmp[tail] = tmp[tail + 1] + tmp[tail + 1].DirectionTo(Body.Points[tail] - velocity) * SegmentLength;

                if (HurtBoxes.ContainsKey(tail))
                {
                    HurtBoxes[tail].Position = tmp[tail];
                }
            }

            int head = CaughtHurtBoxIndex + i;
            if (head < Length)
            {
                tmp[head] = tmp[head - 1] + tmp[head - 1].DirectionTo(Body.Points[head] - velocity) * SegmentLength;

                if (HurtBoxes.ContainsKey(head))
                {
                    HurtBoxes[head].Position = tmp[head];
                }
            }

        }

        Body.Points = tmp;

        Sprite.Position = Body.Points[0] + Body.Points[0].DirectionTo(Body.Points[1]) * -28;
        Sprite.Rotation = Body.Points[0].AngleToPoint(Body.Points[1]) - Mathf.Pi;
    }

    public override IFishable GetCaughtBy(IFisher by)
    {
        if (GetCaughtBySafetyGuard(by))
            return this;
        AnchorPoint = (by as Node2D).FindChild("HitBox", true, false) as Node2D;
        AnchorPointLastPosition = AnchorPoint.GlobalPosition;

        float minDistance = float.MaxValue;

        foreach (var hurtBox in HurtBoxes)
        {

            float distance = hurtBox.Value.GlobalPosition.DistanceTo(AnchorPoint.GlobalPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                CaughtHurtBoxIndex = hurtBox.Key;
            }
        }

        HurtBoxes[CaughtHurtBoxIndex].DebugColor = Colors.Black;

        return base.GetCaughtBy(by);
    }
}
