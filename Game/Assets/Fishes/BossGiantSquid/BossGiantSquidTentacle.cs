using Godot;
using System;
using System.Collections.Generic;
using WaffleStock;

public partial class BossGiantSquidTentacle : Fish
{
    [Signal]
    public delegate void GotCaughtEventHandler();

    [Export]
    private Line2D Body;

    private uint Length;
    private uint SegmentLength = 10;

    [Export]
    private Curve AmplitudeCurve;
    [Export]
    private int WaveAmplitude = 150;
    [Export]
    private int WaveSpeed = 5;

    private float Seed;

    public float CatchRate = 1;

    private Dictionary<int, CollisionShape2D> HurtBoxes = new();

    public Node2D AnchorPoint = null;
    private Vector2 AnchorPointLastPosition = Vector2.Zero;
    private int CaughtHurtBoxIndex = 0;


    public override void _Ready()
    {
        // We skip the base ready function because even though it's of fish class for convenience
        // it is obvously not an autonomous fish at all
        //base._Ready();

        AnchorPoint = (Node2D)GetParent();

        Length = (uint)Math.Abs(Body.Points[Body.Points.Length - 1].X / SegmentLength);

        HurtBoxes[0] = GetNode<CollisionShape2D>("HurtBox");

        Seed = GD.RandRange(0, 1000);

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

        Body.Points = new Vector2[Length];
    }

    int test = 0;
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
            tmp[i] = new Vector2(Sprite.Position.X - SegmentLength * i,
                Sprite.Position.Y +
                (float)
                Math.Sin((((GlobalPosition.X + Seed) / 300f) - (SegmentLength * (Length - i))) * WaveSpeed

                + Math.Abs(Position.Y)
                // This mirrors up and down tentacles
                + (Position.Y > 0 ? Math.PI : 0))
               * (WaveAmplitude * AmplitudeCurve.Sample((float)i / Length)));
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
    }

    public override IFishable GetCaughtBy(IFisher by)
    {
        if (GD.Randf() > CatchRate)
            return this;

        if (GetCaughtBySafetyGuard(by))
            return this;

        EmitSignalGotCaught();

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

    protected override void Despawn()
    {
        if (!(GetParent() is BossGiantSquid))
            base.Despawn();
    }
}
