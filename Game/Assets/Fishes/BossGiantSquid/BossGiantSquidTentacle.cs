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

    }

    public override IFishable GetCaughtBy(IFisher by)
    {
        return base.GetCaughtBy(by);

    }
}
