using Godot;
using WaffleStock;
using System;
using System.Collections.Generic;

public partial class SerpentFish : Fish
{
    [Export]
    Line2D Body;
    [Export]
    uint Length = 50;
    [Export]
    uint SegmentLength = 10;
    [Export]
    int HeadOffset = -28;
    [Export]
    Curve AmplitudeCurve;
    [Export]
    int WaveAmplitude = 60;
    [Export]
    int WaveSpeed = 10;

    private Dictionary<int, CollisionShape2D> HurtBoxes = new();

    private DateTime InstanciationTime = DateTime.Now;

    public override void _Ready()
    {
        base._Ready();
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

        GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D").Rect = new Rect2(-SegmentLength * Length, -WaveAmplitude, SegmentLength * Length, WaveAmplitude * 2);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!IsActionable && !IsInDisplay) return;
        Vector2[] tmp = new Vector2[Length];

        tmp[0] = new Vector2(Sprite.Position.X + HeadOffset, Sprite.Position.Y);

        for (int i = 1; i < Length; i++)
        {
            tmp[i] = new Vector2(Sprite.Position.X + HeadOffset - SegmentLength * i,
                Sprite.Position.Y + (float)Math.Sin((((DateTime.Now - InstanciationTime).TotalMilliseconds / 1000f) - (SegmentLength * (Length - i))) * WaveSpeed) * (WaveAmplitude * AmplitudeCurve.Sample((float)i / Length)));
            Body.Points = tmp;
            if (HurtBoxes.ContainsKey(i))
            {
                HurtBoxes[i].Position = tmp[i];
            }
        }
    }

    public override IFishable GetCaughtBy(IFisher by)
    {

        return base.GetCaughtBy(by);
    }
}
