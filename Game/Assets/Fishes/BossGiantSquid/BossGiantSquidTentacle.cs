using Godot;
using System;
using System.Collections.Generic;
using WaffleStock;

public partial class BossGiantSquidTentacle : Fish
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

    private Node2D AnchorPoint = null;
    private Vector2 AnchorPointLastPosition = Vector2.Zero;
    private int CaughtHurtBoxIndex = 0;


    public override void _Ready()
    {
        // We skip the base ready function because even though it's of fish class for convenience
        // it is obvously not an autonomous fish at all
        //base._Ready();

        HurtBoxes[0] = GetNode<CollisionShape2D>("HurtBox");

        int lasti = 0;
        float radius = (HurtBoxes[0].Shape as CircleShape2D).Radius;
        for (int i = 1; i < Length; i++)
        {
            if ((i - lasti) * SegmentLength > radius)
            {
                CollisionShape2D shape = new();
                shape.Shape = new CircleShape2D();

                (shape.Shape as CircleShape2D).Radius = radius;

                shape.AddToGroup("Hurtboxes");
                HurtBoxes[i] = shape;
                AddChild(shape);
                lasti = i;
            }
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (IsCaught)
        {
            //GetDragged();
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
            tmp[i] = new Vector2(Sprite.Position.X + HeadOffset - SegmentLength * i,
                Sprite.Position.Y + (float)Math.Sin((((DateTime.Now - InstanciationTime).TotalMilliseconds / 1000f) - (SegmentLength * (Length - i))) * WaveSpeed) * (WaveAmplitude * AmplitudeCurve.Sample((float)i / Length)));
            if (HurtBoxes.ContainsKey(i))
            {
                HurtBoxes[i].Position = tmp[i];
            }
        }

        Body.Points = tmp;
    }

    public override IFishable GetCaughtBy(IFisher by)
    {
        return base.GetCaughtBy(by);

    }
}
